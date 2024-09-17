using Cocona;
using StorageSenser;

var builder = CoconaApp.CreateBuilder();
var app = builder.Build();

app.AddCommand((float threshold,bool parallel) =>
{
    var directory = Directory.GetCurrentDirectory();

    StorageSense senser = !parallel
        ? new SimpleStorageSense(directory, threshold)
        : new ParallelStorageSense(directory, threshold);

    var files = senser.SenseStorage();
    foreach (var file in files)
    {
        Console.WriteLine(file);
    }
    Console.WriteLine($"Found: {files.Count} file.");
});

app.Run();
