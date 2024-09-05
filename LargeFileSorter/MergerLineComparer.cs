namespace LargeFileSorter;

class MergerLineComparer : IComparer<(string line, int readerIndex)>
{
	private readonly IComparer<string> lineComparer = new LineComparer();

	public int Compare((string line, int readerIndex) x, (string line, int readerIndex) y)
	{
		int lineComparison = lineComparer.Compare(x.line, y.line);
		if (lineComparison == 0)
		{
			// If lines are the same, compare readerIndex to avoid key duplication
			return x.readerIndex.CompareTo(y.readerIndex);
		}
		return lineComparison;
	}
}