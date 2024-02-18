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
        var chunkPaths = CreatedSortedChunks(sourceDirectory, sourceFileName);

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

        return File.ReadLines(Path.Combine(sourceDirectory, sourceFileName))
            .AsParallel()
            .Select(x => new Item(x))
            .Chunk(ChunkSize)
            .AsParallel()
            .Select(chunk =>
            {
                chunk.AsSpan().Sort();
                var chunkNumber = Interlocked.Increment(ref chunkCounter);
                var chunkFilePath = Path.Combine(sourceDirectory, ChunksDirectoryName, $"chunk_{chunkNumber}.txt");
                File.WriteAllLines(chunkFilePath, chunk.Select(x => x.Source));
                Console.WriteLine($"{DateTime.Now}:_{chunkFilePath}");
                return chunkFilePath;
            }).AsUnordered().ToArray();
    }

    private int ChunkSize => 1_000_000;
}