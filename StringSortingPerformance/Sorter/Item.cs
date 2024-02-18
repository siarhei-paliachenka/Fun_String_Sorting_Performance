namespace Sorter;

public record Item : IComparable<Item>
{
    public Item(string source)
    {
        Source = source;
        SpaceDividerIndex = GetSpaceDividerIndex(source);
    }

    private int GetSpaceDividerIndex(string source)
    {
        return source.IndexOf(' ', StringComparison.InvariantCulture);
    }

    public string Source { get; }

    private int SpaceDividerIndex { get; }
    private int Index => int.Parse(Source.AsSpan().Slice(0, SpaceDividerIndex - 1));

    private ReadOnlySpan<char> Text => Source.AsSpan().Slice(SpaceDividerIndex + 1);

    public int CompareTo(Item? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        var textComparison = Text.CompareTo(other.Text, StringComparison.Ordinal);
        if (textComparison != 0) return textComparison;
        return Index.CompareTo(other.Index);
    }
}