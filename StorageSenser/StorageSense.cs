using System.Diagnostics;

namespace StorageSenser;

public abstract class StorageSense
{
    public string DirectoryPath { get; private set; }
    protected readonly List<File> _bigFiles;
    protected readonly float _threshold;

    public StorageSense(string directoryPath, float threshold)
    {
        DirectoryPath = directoryPath;
        _bigFiles = new List<File>();
        _threshold = threshold;
    }

    public List<File> SenseStorage()
    {
        _bigFiles.Clear();
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        TraverseFileTree(DirectoryPath, (file) =>
        {
            if(file.LengthInMb > _threshold)
                _bigFiles.Add(file);
        });
        stopWatch.Stop();
        Console.WriteLine($"elapsed time in sec:{stopWatch.Elapsed.TotalSeconds}");
        return _bigFiles;
    }

    public abstract void TraverseFileTree(string path, Action<File> checkFile);
}