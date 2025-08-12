using System.Text;

namespace Progress.Components;

internal class Bar : IComponent
{
    private readonly decimal _items;
    private readonly decimal _count;
    private readonly char _progressSymbol;
    private readonly uint _width;
    private readonly char[] _bar;

    private decimal _percent;
    private int _adjustedPercent;

    public Bar(ulong availableItems, ulong count, uint width, char progressSymbol)
    {
        _items = availableItems;
        _count = count;
        _progressSymbol = progressSymbol;
        _width = width;
        _bar = new char[width];
    }

    public bool DisplayPercent { get; set; } = true;

    private void Fill()
    {
        _percent = _count / _items * 100;
        _adjustedPercent = (int)(_count / _items * _width);
    }

    public override string ToString()
    {
        Fill();
        Array.Fill(_bar, _progressSymbol, 0, _adjustedPercent);
        Array.Fill(_bar, ' ', _adjustedPercent, (int)(_width - _adjustedPercent));

        StringBuilder sBuilder = new();
        sBuilder.Append('[');

        foreach(var item in _bar)
            sBuilder.Append(item);

        sBuilder.Append(']');

        if (DisplayPercent)
        {
            sBuilder.Append(' ');
            sBuilder.Append(_percent.ToString("0.00"));
            sBuilder.Append(" %");
        }

        return sBuilder.ToString();
    }

    public IComponent Next(ulong availableItems, ulong currentCount)
    {
        return new Bar(availableItems, currentCount, _width, _progressSymbol);
    }
}
