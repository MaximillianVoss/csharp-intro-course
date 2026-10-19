namespace Course.Lesson16;

public static class Task03
{
    public static bool TryPort(string? text, out int port)
    {
        port = 0;
        if (text is null || text.Length is < 1 or > 5) return false;
        int value = 0;
        foreach (char c in text) { if (c is < '0' or > '9') return false; value = value * 10 + c - '0'; }
        if (value is < 1 or > 65535) return false;
        port = value; return true;
    }
    public static void Run()
    {
        (string? Text,bool Success,int Port)[] cases = {
            ("1",true,1),("00443",true,443),("65535",true,65535),("0",false,0),
            ("65536",false,0),("+80",false,0),(" 80",false,0),("８０",false,0),
            ("123456",false,0),("",false,0),(null,false,0) };
        foreach (var row in cases) { Check.Equal(row.Success,TryPort(row.Text,out int port)); Check.Equal(row.Port,port); }
        foreach (var row in new (string Text,bool Success,int Port)[] {
            ("00001",true,1),("00000",false,0),("000001",false,0),("80 ",false,0),("8a0",false,0) })
        {
            Check.Equal(row.Success,TryPort(row.Text,out int port)); Check.Equal(row.Port,port);
        }
    }
}
