param([switch]$AllowPartial)
$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
$artifacts = Join-Path $root 'artifacts'
New-Item -ItemType Directory -Path $artifacts -Force | Out-Null

function Invoke-Dotnet {
    param([string[]]$Arguments, [string]$LogName, [int]$TimeoutSeconds = 180)
    $info = [System.Diagnostics.ProcessStartInfo]::new('dotnet')
    $info.WorkingDirectory = $root
    $info.UseShellExecute = $false
    $info.CreateNoWindow = $true
    $info.RedirectStandardOutput = $true
    $info.RedirectStandardError = $true
    $info.StandardOutputEncoding = [System.Text.Encoding]::UTF8
    $info.StandardErrorEncoding = [System.Text.Encoding]::UTF8
    foreach ($argument in $Arguments) { $info.ArgumentList.Add($argument) }
    $process = [System.Diagnostics.Process]::new()
    $process.StartInfo = $info
    [void]$process.Start()
    $stdoutTask = $process.StandardOutput.ReadToEndAsync()
    $stderrTask = $process.StandardError.ReadToEndAsync()
    if (-not $process.WaitForExit($TimeoutSeconds * 1000)) {
        $process.Kill($true)
        throw "Timeout: dotnet $($Arguments -join ' ')"
    }
    $stdout = $stdoutTask.GetAwaiter().GetResult()
    $stderr = $stderrTask.GetAwaiter().GetResult()
    $stdout + $stderr | Set-Content -LiteralPath (Join-Path $artifacts "$LogName.log") -Encoding utf8
    if ($process.ExitCode -ne 0) {
        Write-Host $stdout
        Write-Host $stderr
        throw "dotnet failed ($($process.ExitCode)): $($Arguments -join ' ')"
    }
    return $stdout
}
$manifests = @(Get-ChildItem -LiteralPath (Join-Path $root 'lessons') -Filter 'tasks.json' -Recurse | Sort-Object FullName)
if (-not $AllowPartial -and $manifests.Count -ne 16) { throw "Expected 16 modules, got $($manifests.Count)." }
Write-Host 'Restore and Release build...'
$null = Invoke-Dotnet -Arguments @('restore','Курс CSharp.slnx','--locked-mode','--verbosity','minimal') -LogName 'restore'
$null = Invoke-Dotnet -Arguments @('build','Курс CSharp.slnx','-c','Release','--no-restore','--verbosity','minimal','-warnaserror') -LogName 'build'
Write-Host 'MSTest...'
$null = Invoke-Dotnet -Arguments @('test','capstone/Библиотека.Тесты/Библиотека.Тесты.csproj','-c','Release','--no-build','--no-restore','--logger','trx;LogFileName=capstone.trx','--results-directory','artifacts/TestResults') -LogName 'mstest'
[xml]$trx = Get-Content -LiteralPath (Join-Path $artifacts 'TestResults/capstone.trx') -Raw
$counters = $trx.TestRun.ResultSummary.Counters
if ([int]$counters.total -eq 0 -or [int]$counters.failed -ne 0 -or [int]$counters.passed -ne [int]$counters.total) { throw 'Tests were failed, missing or skipped.' }

$lessonResults = @()
$taskTotal = 0
$assertionTotal = 0
foreach ($manifest in $manifests) {
    $lesson = Get-Content -LiteralPath $manifest.FullName -Raw | ConvertFrom-Json
    $number = '{0:00}' -f [int]$lesson.number
    $folder = Split-Path -Parent $manifest.FullName
    Write-Host "Module $number..."
    $exampleDll = Join-Path $folder "examples/bin/Release/net10.0/Урок$number.Примеры.dll"
    $solutionDll = Join-Path $folder "solutions/bin/Release/net10.0/Урок$number.Решения.dll"
    $templateDll = Join-Path $folder "templates/bin/Release/net10.0/Урок$number.Заготовки.dll"
    $null = Invoke-Dotnet -Arguments @('exec',$exampleDll) -LogName "example-$number" -TimeoutSeconds 30
    $solutions = Invoke-Dotnet -Arguments @('exec',$solutionDll,'--all') -LogName "solutions-$number" -TimeoutSeconds 60
    $line = @($solutions -split "\r?\n" | Where-Object { $_.StartsWith('RESULT ') })
    if ($line.Count -ne 1) { throw "Missing RESULT for module $number." }
    $result = $line[0].Substring(7) | ConvertFrom-Json
    if ($result.tasks -ne 15 -or $result.failed -ne 0 -or $result.assertions -lt 15 -or $result.template) { throw "Incomplete solutions in module $number." }
    $taskTotal += [int]$result.tasks
    $assertionTotal += [int]$result.assertions
    $listed = Invoke-Dotnet -Arguments @('exec',$templateDll,'--list') -LogName "templates-$number" -TimeoutSeconds 30
    $ids = @($listed -split "\r?\n" | Where-Object { $_ -match '^\d{2}-\d{2}$' })
    if ($ids.Count -ne 15) { throw "Template catalog mismatch in module $number." }
    $lessonResults += [ordered]@{lesson=[int]$lesson.number; tasks=[int]$result.tasks; assertions=[int]$result.assertions; example='passed'; templates='compiled_and_listed_only'}
}
$report = [ordered]@{
    sdk=(& dotnet --version)
    platform=[System.Runtime.InteropServices.RuntimeInformation]::OSDescription
    release_build='passed_with_warnings_as_errors'
    mstest=[ordered]@{total=[int]$counters.total; passed=[int]$counters.passed; failed=[int]$counters.failed}
    lessons=$lessonResults
    tasks=$taskTotal
    assertions=$assertionTotal
    examples=$lessonResults.Count
    template_projects=$lessonResults.Count
    templates_claimed_solved=$false
}
$report | ConvertTo-Json -Depth 8 | Set-Content -LiteralPath (Join-Path $artifacts 'verification.json') -Encoding utf8
Write-Host "Verified $($report.tasks) tasks, $($report.assertions) assertions, $($report.examples) examples; MSTest $($counters.passed)/$($counters.total)."
