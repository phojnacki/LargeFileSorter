namespace LargeFileSorter;
class LineComparer : IComparer<string>
{
	public int Compare(string x, string y)
	{
		(int numberX, string stringX) = ParseLine(x);
		(int numberY, string stringY) = ParseLine(y);

		int stringCompare = string.Compare(stringX, stringY);
		if (stringCompare == 0)
		{
			return numberX.CompareTo(numberY);
		}
		return stringCompare;
	}

	public (int, string) ParseLine(string input)
	{
		int separatorIndex = input.IndexOf(". ");
		if (separatorIndex == -1)
		{
			throw new ArgumentException($"Input '{input}' does not contain the expected separator '. '");
		}
		string integerPartStr = input.Substring(0, separatorIndex);
		if (!int.TryParse(integerPartStr, out int integerPart))
		{
			throw new ArgumentException($"The input '{input}' does not start with a valid integer.");
		}
		string stringPart = input.Substring(separatorIndex + 2);

		return (integerPart, stringPart);
	}

}