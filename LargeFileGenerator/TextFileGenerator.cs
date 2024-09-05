using System.Text;

namespace LargeFileGenerator;

class TextFileGenerator
{
	private readonly List<string> _strings;
	private readonly long _targetFileSize;

	private TextFileGenerator(Builder builder)
	{
		_strings = builder._strings;
		_targetFileSize = builder._targetFileSize;
	}

	public void GenerateFile(string filePath)
	{
		Random random = new Random();
		long currentFileSize = 0;

		using (var writer = new StreamWriter(filePath))
		{
			while (currentFileSize < _targetFileSize)
			{
				int number = random.Next(1, 1000);
				string randomString = _strings[random.Next(_strings.Count)];

				string line = $"{number}. {randomString}";
				writer.WriteLine(line);

				currentFileSize += Encoding.UTF8.GetByteCount(line + Environment.NewLine);
			}
		}
	}

	public class Builder
	{
		internal List<string> _strings = new List<string>();
		internal long _targetFileSize;

		public Builder WithTargetFileSize(long targetFileSize)
		{
			_targetFileSize = targetFileSize;
			return this;
		}

		public Builder AddRepeatedString(string value)
		{
			_strings.Add(value);
			return this;
		}

		public TextFileGenerator Build()
		{
			if (_strings.Count == 0)
			{
				throw new InvalidOperationException("No strings added. Please add at least one string using AddRepeatedString.");
			}

			if (_targetFileSize <= 0)
			{
				throw new InvalidOperationException("Invalid target file size. It must be greater than zero.");
			}

			return new TextFileGenerator(this);
		}
	}
}