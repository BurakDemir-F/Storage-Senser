using System.Diagnostics;

namespace StorageSenser;

public class SimpleStorageSense : StorageSense
{
    public SimpleStorageSense(string directoryPath, float threshold) : base(directoryPath, threshold)
    {
    }

    public override void TraverseFileTree(string path, Action<File> checkFile)
    {
        var files = new List<string>();
        var subDirectories = new List<string>();
        var directories = new Stack<string>();

        var currentFolder = path;
        directories.Push(currentFolder);
        while (directories.Count > 0)
        {
            currentFolder = directories.Pop();
            files.Clear();
            subDirectories.Clear();
            
            try
            {
                foreach (var file in Directory.EnumerateFiles(currentFolder))
                    files.Add(file);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
                continue;
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                continue;                
            }

            try
            {
                foreach (var directory in Directory.EnumerateDirectories(currentFolder))
                    subDirectories.Add(directory);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
                continue;
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                continue;                
            }
            
            foreach (var file in files)
            {
                checkFile?.Invoke(new File(file));
            }
            
            foreach (var subDirectory in subDirectories)
                directories.Push(subDirectory);
        }
        
    }
}