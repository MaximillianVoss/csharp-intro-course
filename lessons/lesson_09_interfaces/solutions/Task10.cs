namespace Course.Lesson09;

public static class Task10
{
    public interface INode { long Size { get; } }
    public sealed class FileNode : INode
    {
        public long Size { get; } public FileNode(long size) { Size = size; }
    }
    public sealed class FolderNode : INode
    {
        private readonly List<INode> children = new();
        public void Add(INode node) { children.Add(node); }
        public long Size { get { long total = 0; foreach (INode child in children) total += child.Size; return total; } }
    }
    public static void Run()
    {
        FolderNode root = new(); FolderNode nested = new(); nested.Add(new FileNode(5)); nested.Add(new FileNode(7));
        root.Add(new FileNode(3)); root.Add(nested); INode view = root; Check.Equal(15L, view.Size);
        Check.Equal(0L, new FolderNode().Size); nested.Add(new FileNode(1)); Check.Equal(16L, root.Size);
        FolderNode large = new(); for (int i = 0; i < 3; i++) large.Add(new FileNode(1_000_000_000));
        Check.Equal(3_000_000_000L, large.Size);
    }
}
