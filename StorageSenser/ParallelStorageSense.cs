using System.Diagnostics;

namespace StorageSenser;

public class ParallelStorageSense : StorageSense
{
    public ParallelStorageSense(string directoryPath, float threshold) : base(directoryPath, threshold)
    {
    }

    public override void TraverseFileTree(string path, Action<File> checkFile)
    {
        int fileCount = 0;

        // Determine whether to parallelize file processing on each folder based on processor count.
        int procCount = Environment.ProcessorCount;

        // Data structure to hold names of subfolders to be examined for files.
        Stack<string> dirs = new Stack<string>();
        dirs.Push(path);

        var subDirs = new List<string>();
        var files = new List<string>();

        while (dirs.Count > 0)
        {
            string currentDir = dirs.Pop();
            subDirs.Clear();
            files.Clear();

            try
            {
                foreach (var directory in Directory.GetDirectories(currentDir))
                {
                    subDirs.Add(directory);
                }
            }
            // Thrown if we do not have discovery permission on the directory.
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
                continue;
            }
            // Thrown if another process has deleted the directory after we retrieved its name.
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
                continue;
            }

            try
            {
                foreach (var file in Directory.GetFiles(currentDir))
                {
                    files.Add(file);
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
                continue;
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
                continue;
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                continue;
            }

            // Execute in parallel if there are enough files in the directory.
            // Otherwise, execute sequentially.Files are opened and processed
            // synchronously but this could be modified to perform async I/O.
            try
            {
                if (files.Count < procCount)
                {
                    foreach (var file in files)
                    {
                        checkFile(new File(file));
                        fileCount++;
                    }
                }
                else
                {
                    Parallel.ForEach(files, () => 0,
                        (file, loopState, localCount) =>
                        {
                            checkFile(new File(file));
                            return (int)++localCount;
                        },
                        (c) => { Interlocked.Add(ref fileCount, c); });
                }
            }
            catch (AggregateException ae)
            {
                ae.Handle((ex) =>
                {
                    if (ex is UnauthorizedAccessException)
                    {
                        // Here we just output a message and go on.
                        Console.WriteLine(ex.Message);
                        return true;
                    }
                    // Handle other exceptions here if necessary...

                    return false;
                });
            }

            // Push the subdirectories onto the stack for traversal.
            // This could also be done before handing the files.
            foreach (string str in subDirs)
                dirs.Push(str);
        }
    }
}
