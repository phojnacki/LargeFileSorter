using System.Text;

namespace LargeFileGenerator;

class Program
{
	static void Main(string[] args)
	{
		TextFileGenerator generator = new TextFileGenerator.Builder()
										.WithTargetFileSize(100_000_000) // Bytes
										.AddRepeatedString("apples")
										.AddRepeatedString("potatoes")
										.AddRepeatedString("citrons")
										.AddRepeatedString("plums")
										.AddRepeatedString("cherrys")
										.AddRepeatedString("something something")
										.AddRepeatedString("pears")
										.AddRepeatedString("pineapples")
										.AddRepeatedString("nothing")
										.AddRepeatedString("else")
										.Build();

		generator.GenerateFile("largefile.txt");

		Console.WriteLine("File generation completed.");
	}
}