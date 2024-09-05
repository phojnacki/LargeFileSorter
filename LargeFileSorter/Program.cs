using System.Diagnostics;

namespace LargeFileSorter;

public class Program
{
	static void Main(string[] args)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();

		var splitter = new LargeFileSplitter.Builder()
			.WithInputFilePath("files\\largefile.txt")
			.WithLinesPerChunk(100_000_000)
			.Build();
		List<string> sortedChunkTempFiles = splitter.SplitAndSortChunks();

		var merger = new SortedChunksMerger.Builder()
			.WithOutputFilePath("files\\sortedfile.txt")
			.WithChunkTempFiles(sortedChunkTempFiles)
			.Build();
		merger.MergeSortedChunks();

		stopwatch.Stop();
		TimeSpan elapsedTime = stopwatch.Elapsed;
		Console.WriteLine($"Time taken: {elapsedTime.TotalSeconds} seconds");

		merger.CleanUpTemporaryFiles();
	}
}