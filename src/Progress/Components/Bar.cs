using System.Text;

namespace Progress.Components;

internal class Bar : Component
{
    private readonly char _progressSymbol;
    private readonly uint _width;
    private readonly char[] _bar;

    private ulong _availableItems;
    private ulong _count;
    private int _adjustedPercent;

    public Bar(uint width, char progressSymbol)
    {
        _progressSymbol = progressSymbol;
        _width = width;
        _bar = new char[width];
    }

    public bool DisplayPercent { get; init; } = true;

    public override Component Next(ulong availableItems, ulong currentCount)
    {
        _availableItems = availableItems;
        _count = currentCount;
        CurrentPercent = Calculate(availableItems, currentCount);
        return this;
    }

    public override string ToString()
    {
        Fill();

        StringBuilder sBuilder = new();
        sBuilder.Append('[');

        foreach(var item in _bar)
            sBuilder.Append(item);

        sBuilder.Append(']');

        if (DisplayPercent)
        {
            sBuilder.Append(' ');
            sBuilder.Append(CurrentPercent.ToString());
        }

        return sBuilder.ToString();
    }

    private void Fill()
    {
        if (CurrentPercent.Value > 0)
            _adjustedPercent = (int)(_count / (decimal)_availableItems * _width);

        if (_adjustedPercent <= _bar.Length)
        {
            Array.Fill(_bar, _progressSymbol, 0, _adjustedPercent);
            Array.Fill(_bar, ' ', _adjustedPercent, (int)(_width - _adjustedPercent));
        }
    }
}
