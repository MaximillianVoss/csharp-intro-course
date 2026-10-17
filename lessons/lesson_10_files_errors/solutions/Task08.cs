namespace Course.Lesson10;

public static class Task08
{
    public sealed class TokenLease : IDisposable
    {
        private readonly string token; private bool disposed;
        public int ReleaseCount { get; private set; }
        public TokenLease(string token) { this.token = token; }
        public string Read()
        {
            if (disposed) throw new ObjectDisposedException(nameof(TokenLease));
            return token;
        }
        public void Dispose() { if (disposed) return; disposed = true; ReleaseCount++; }
    }
    public static void Run()
    {
        TokenLease lease = new("abc"); Check.Equal("abc", lease.Read()); lease.Dispose(); lease.Dispose();
        Check.Equal(1, lease.ReleaseCount); Check.Throws<ObjectDisposedException>(() => lease.Read());
        TokenLease normal = new("x"); using (normal) { Check.Equal("x", normal.Read()); } Check.Equal(1, normal.ReleaseCount);
        TokenLease failed = new("y");
        Check.Throws<InvalidOperationException>(() => { using (failed) { throw new InvalidOperationException(); } });
        Check.Equal(1, failed.ReleaseCount); Check.Throws<ObjectDisposedException>(() => failed.Read());
    }
}
