using System.Diagnostics;

namespace TestDataGenerator;

public class Generator
{
    private const int AmountOfBiteInOneGigabyte = 1_073_741_824;

    public async Task GenerateTestFile(string sourceDirectory,
        string sourceFileName,
        string resultFileName,
        double fileSizeInGigabytes,
        int lineMaxLength,
        CancellationToken cancellationToken)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        
        var sourcePath = Path.Combine(sourceDirectory, sourceFileName);
        var lines = ReadOriginalText(sourcePath, cancellationToken);

        var preparedLines = PrepareLines(lines, lineMaxLength);

        var resultFilePath = GetResultFilePath(sourceDirectory, resultFileName, fileSizeInGigabytes);

        CreateFile(resultFilePath);

        do
        {
            var numberedLines = AddLineNumber(preparedLines);
            await WriteInFile(resultFilePath, numberedLines);
        } while (!IsFileSizeAchieved(resultFilePath, fileSizeInGigabytes));
        
        stopwatch.Stop();

        Console.WriteLine($"File {resultFilePath} is created in {stopwatch.Elapsed.Seconds} s");
    }

    private static string GetResultFilePath(string sourceDirectory, string resultFileName, double fileSizeInGigabytes)
    {
        return Path.Combine(sourceDirectory, $"{resultFileName}_{fileSizeInGigabytes}Gb.txt");
    }

    private IAsyncEnumerable<string> ReadOriginalText(string path, CancellationToken cancellationToken)
    {
        return File.ReadLinesAsync(path, cancellationToken);
    }

    private void CreateFile(string path)
    {
        var file = File.Create(path);
        file.Close();
    }

    private async Task WriteInFile(string path, IAsyncEnumerable<string> content)
    {
        await using var stream = File.AppendText(path);

        var lines = await content.ToArrayAsync();
        
        Random.Shared.Shuffle(lines.AsSpan());
        
        foreach (var line in lines)
        {
            await stream.WriteLineAsync(line);
        }
    }

    private long GetFileSize(string path) => new FileInfo(path).Length;

    private IAsyncEnumerable<string> PrepareLines(IAsyncEnumerable<string> lines, int lineMaxLength)
    {
        return lines.Where(x => x.Any())
            .Select(x => x.Chunk(lineMaxLength).Select(c => new string(c)))
            .SelectMany(x => x.ToAsyncEnumerable());
    }

    private async IAsyncEnumerable<string> AddLineNumber(IAsyncEnumerable<string> lines)
    {
        await foreach (var line in lines)
        {
            yield return $"{Random.Shared.Next()}. {line}";
        }
    }

    private bool IsFileSizeAchieved(string path, double fileSizeInGigabytes)
    {
        double expectedSize = fileSizeInGigabytes * AmountOfBiteInOneGigabyte;
        long actualSize = GetFileSize(path);
        Console.WriteLine($"File size is {actualSize} Gb");
        return  actualSize > expectedSize;
    }
}