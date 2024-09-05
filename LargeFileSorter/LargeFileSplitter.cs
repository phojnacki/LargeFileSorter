namespace LargeFileSorter;

class LargeFileSplitter
{
	private readonly string inputFilePath;
	private readonly int chunkSize;
	private readonly LineComparer lineComparer = new();

	private LargeFileSplitter(string inputFilePath, int chunkSize)
	{
		this.inputFilePath = inputFilePath;
		this.chunkSize = chunkSize;
	}

	public List<string> SplitAndSortChunks()
	{
		List<string> tempFiles = new List<string>();
		using (var sr = new StreamReader(inputFilePath))
		{
			int fileCounter = 0;
			while (!sr.EndOfStream)
			{
				List<string> lines = new List<string>();
				for (int i = 0; i < chunkSize && !sr.EndOfStream; i++)
				{
					lines.Add(sr.ReadLine());
				}

				lines.Sort(lineComparer);

				string tempFilePath = $"files\\temp_{fileCounter}.txt";
				Console.WriteLine($"Writing {tempFilePath}");
				File.WriteAllLines(tempFilePath, lines);
				tempFiles.Add(tempFilePath);

				fileCounter++;
			}
		}
		return tempFiles;
	}

	public class Builder
	{
		private string inputFilePath;
		private int chunkSize;

		public Builder WithInputFilePath(string inputFilePath)
		{
			this.inputFilePath = inputFilePath;
			return this;
		}

		public Builder WithLinesPerChunk(int chunkSize)
		{
			this.chunkSize = chunkSize;
			return this;
		}

		public LargeFileSplitter Build()
		{
			return new LargeFileSplitter(inputFilePath, chunkSize);
		}
	}
}