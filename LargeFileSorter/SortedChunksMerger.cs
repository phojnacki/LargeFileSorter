namespace LargeFileSorter;
class SortedChunksMerger
{
	private readonly string outputFilePath;
	private readonly List<string> tempFiles;
	private readonly MergerLineComparer lineComparer = new();

	private SortedChunksMerger(string outputFilePath, List<string> tempFiles)
	{
		this.outputFilePath = outputFilePath;
		this.tempFiles = tempFiles;
	}

	public void MergeSortedChunks()
	{
		List<StreamReader> readers = tempFiles.Select(f => new StreamReader(f)).ToList();
		using (var sw = new StreamWriter(outputFilePath))
		{
			var sortedSet = new SortedSet<(string line, int readerIndex)>(lineComparer);

			InitializeSet(readers, sortedSet);

			InternalMerge(readers, sw, sortedSet);
		}

		CloseReaders(readers);
		Console.WriteLine($"Sorting completed. Output file: {outputFilePath}");
	}

	private void InternalMerge(List<StreamReader> readers, StreamWriter sw, SortedSet<(string line, int readerIndex)> sortedSet)
	{
		while (sortedSet.Count > 0)
		{
			var mergerEntry = sortedSet.First();
			string line = mergerEntry.line;
			int readerIndex = mergerEntry.readerIndex;

			sw.WriteLine(line);

			sortedSet.Remove(mergerEntry);

			if (!readers[readerIndex].EndOfStream)
			{
				string nextLine = readers[readerIndex].ReadLine();
				sortedSet.Add((nextLine, readerIndex));
			}
		}
	}

	private void InitializeSet(List<StreamReader> readers, SortedSet<(string line, int readerIndex)> sortedSet)
	{
		for (int i = 0; i < readers.Count; i++)
		{
			if (!readers[i].EndOfStream)
			{
				string lineToAdd = readers[i].ReadLine();
				sortedSet.Add((lineToAdd, i));
			}
		}
	}

	public void CleanUpTemporaryFiles()
	{
		foreach (var tempFile in tempFiles)
		{
			File.Delete(tempFile);
		}
	}
	private void CloseReaders(List<StreamReader> readers)
	{
		foreach (var reader in readers)
		{
			reader.Close();
		}
	}

	public class Builder
	{
		private string outputFilePath;
		private List<string> tempFiles;

		public Builder WithOutputFilePath(string outputFilePath)
		{
			this.outputFilePath = outputFilePath;
			return this;
		}

		public Builder WithChunkTempFiles(List<string> tempFiles)
		{
			this.tempFiles = tempFiles;
			return this;
		}

		public SortedChunksMerger Build()
		{
			return new SortedChunksMerger(outputFilePath, tempFiles);
		}
	}
}