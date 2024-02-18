namespace Sorter;

public record struct Item : IComparable<Item>
{
    public Item(string source)
    {
        Source = source;
        SpaceDividerIndex = GetSpaceDividerIndex(source);
    }

    private int GetSpaceDividerIndex(string source)
    {
        return source.IndexOf(' ', StringComparison.Ordinal);
    }

    public string Source { get; }

    private int SpaceDividerIndex { get; }

    private int? _index = null;
    private int Index
    {
        get
        {
            if (_index is null)
            {
                _index = int.Parse(Source.AsSpan().Slice(0, SpaceDividerIndex - 1));
            }
            return _index.Value;
        }
    }

    private ReadOnlySpan<char> Text => Source.AsSpan().Slice(SpaceDividerIndex + 1);

    public int CompareTo(Item other)
    {
        var textComparison = Text.CompareTo(other.Text, StringComparison.Ordinal);
        if (textComparison != 0) return textComparison;
        return Index.CompareTo(other.Index);
    }
}