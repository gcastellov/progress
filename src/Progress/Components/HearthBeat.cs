using System.Text;

namespace Progress.Components;

internal class HearthBeat : IComponent
{
    private readonly char _progressSymbol;
    private readonly char[] _bar;

    private uint _leftIndex;
    private uint _rightIndex;
    private decimal _percent;
    private HearthBeat _hearthBeat = default!;

    public HearthBeat(uint width, char progressSymbol)
    {
        _progressSymbol = progressSymbol;
        _bar = new char[width];
    }

    public bool DisplayPercent { get; set; } = true;

    public IComponent Next(ulong availableItems, ulong currentCount)
    {
        _percent = (decimal)currentCount / (decimal)availableItems * 100;

        if (_leftIndex == 0)
        {
            var (left, right) =  GetInitialIndexes();
            _leftIndex = left;
            _rightIndex = right;
        }
        else
        {
            _leftIndex--;
            _rightIndex++;
        }

        return _hearthBeat ??= this;
    }

    public override string ToString()
    {
        Array.Fill(_bar, ' ');

        if (_percent < 100)
        {
            _bar[_leftIndex] = _progressSymbol;
            _bar[_rightIndex] = _progressSymbol;
        }

        StringBuilder sBuilder = new();
        sBuilder.Append('[');

        foreach (var item in _bar)
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

    private (uint leftIndex, uint rightIndex) GetInitialIndexes()
    {
        uint rightIndex = (uint)_bar.Length / 2;
        uint leftIndex = rightIndex;

        if (_bar.Length % 2 == 0)
            _leftIndex = --rightIndex;

        return (leftIndex, rightIndex);
    }
}
