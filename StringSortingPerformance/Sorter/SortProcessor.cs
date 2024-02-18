using System.Diagnostics;

namespace Sorter;

public class SortProcessor
{
    const string ChunksDirectoryName = "chunks";
    
    public void Sort(string sourceDirectory,
        string sourceFileName,
        string resultFileName,
        CancellationToken cancellationToken)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        
        var sourcePath = Path.Combine(sourceDirectory, sourceFileName);

        CreateChunkDirectory(sourceDirectory);
        var chunkPaths = CreatedSortedChunks(sourceDirectory, sourceFileName).ToArray();
        
        var resultFilePath = Path.Combine(sourceDirectory, resultFileName);
        
        stopwatch.Stop();

        Console.WriteLine($"File {resultFilePath} is created in {stopwatch.Elapsed.Seconds} s");
    }

    private void CreateChunkDirectory(string sourceDirectory)
    {

        var directoryPath = Path.Combine(sourceDirectory, ChunksDirectoryName);
        Directory.CreateDirectory(directoryPath);
    }

    private IEnumerable<string> CreatedSortedChunks(string sourceDirectory, string sourceFileName)
    {
        var chunkCounter = 0;

        return File.ReadLines(Path.Combine(sourceDirectory,sourceFileName))
            .Select(x => new Item(x))
            .Chunk(ChunkSize)
            .Select(chunk =>
            {
                Array.Sort(chunk);
                var chunkFilePath = Path.Combine(sourceDirectory, ChunksDirectoryName, $"chunk_{chunkCounter++}.txt");
                File.WriteAllLines(chunkFilePath, chunk.Select(x => x.Source));
                Console.WriteLine(chunkFilePath);
                return chunkFilePath;
            });
    }

    private int ChunkSize => 1_000_000;

}