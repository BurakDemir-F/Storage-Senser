using System.Text;

namespace StorageSenser;

public class File
{
    public string Path { get; private set; }
    public float LengthInMb { get; private set; }
    private readonly FileInfo _info;
    public string Name => _info.Name;

    public File(string path)
    {
        Path = path;
        _info = new FileInfo(path);
        LengthInMb = _info.Length / 1000000f;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(Name);
        sb.Append(' ');
        sb.Append(Path);
        sb.Append(' ');
        sb.Append(LengthInMb);
        return sb.ToString();
    }
}