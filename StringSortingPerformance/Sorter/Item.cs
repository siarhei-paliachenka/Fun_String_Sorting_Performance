namespace Sorter;

public record Item: IComparable<Item>
{
    public Item(string source)
    {
        Source = source;
        Text = GetText(source);
        Index = GetIndex(source);
    }

    private int GetIndex(string source)
    {
        var indexOfStart = source.IndexOf(' ', StringComparison.InvariantCulture);
        return int.Parse(source.Substring(0, indexOfStart - 1));
    }

    private string GetText(string source)
    {
        var indexOfStart = source.IndexOf(' ', StringComparison.InvariantCulture);
        return source.Substring(indexOfStart);
    }

    public string Source { get; init; }
    public int Index { get; init; }
    
    public string Text { get; init; }

    public int CompareTo(Item? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        var textComparison = string.Compare(Text, other.Text, StringComparison.Ordinal);
        if (textComparison != 0) return textComparison;
        return Index.CompareTo(other.Index);
    }
}