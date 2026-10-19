using Course;
using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;
namespace Course.Lesson15;

public static class Task09
{
    public static async Task<long> CountBytesAsync(Stream input, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(input); token.ThrowIfCancellationRequested(); var buffer = new byte[4]; long count = 0; int read;
        while ((read = await input.ReadAsync(buffer.AsMemory(), token)) != 0) count = checked(count + read); return count;
    }
    private sealed class NoSeek : MemoryStream
    {
        public NoSeek(byte[] bytes) : base(bytes) { }
        public override bool CanSeek => false;
        public override long Length => throw new NotSupportedException();
        public override long Seek(long offset, SeekOrigin loc) => throw new NotSupportedException();
    }
    private sealed class ShortReads : MemoryStream
    {
        public ShortReads(byte[] bytes) : base(bytes) { }
        public int Calls { get; private set; }
        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            Calls++;
            return base.ReadAsync(buffer[..Math.Min(1, buffer.Length)], cancellationToken);
        }
    }
    public static async Task Run()
    {
        using var stream = new MemoryStream(new byte[7]); stream.Position = 2; Check.Equal(5L, await CountBytesAsync(stream)); Check.True(stream.CanRead); Check.Equal(0L, await CountBytesAsync(stream));
        using var noSeek = new NoSeek(new byte[9]); Check.Equal(9L, await CountBytesAsync(noSeek));
        using var cancelled = new MemoryStream(new byte[2]); await Check.ThrowsAsync<OperationCanceledException>(() => CountBytesAsync(cancelled, new CancellationToken(true))); Check.Equal(0L, cancelled.Position);
        await Check.ThrowsAsync<ArgumentNullException>(() => CountBytesAsync(null!));
        using var shortReads = new ShortReads(new byte[5]);
        Check.Equal(5L, await CountBytesAsync(shortReads)); Check.Equal(6, shortReads.Calls); Check.True(shortReads.CanRead);
    }
}
