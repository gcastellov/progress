using System.Text;

namespace Progress.Components;

internal class HearthBeat : Component
{
    private readonly char _progressSymbol;
    private readonly char[] _bar;

    private uint _leftIndex;
    private uint _rightIndex;

    public HearthBeat(uint width, char progressSymbol)
    {
        _progressSymbol = progressSymbol;
        _bar = new char[width];
    }

    public bool DisplayPercent { get; init; } = true;

    public override Component Next(ulong availableItems, ulong currentCount)
    {
        CurrentPercent = Calculate(availableItems, currentCount);

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

        return this;
    }

    public override string ToString()
    {
        Fill();

        StringBuilder sBuilder = new();
        sBuilder.Append('[');

        foreach (var item in _bar)
            sBuilder.Append(item);

        sBuilder.Append(']');

        if (DisplayPercent)
        {
            sBuilder.Append(' ');
            sBuilder.Append(CurrentPercent.ToString());
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

    private void Fill()
    {
        Array.Fill(_bar, ' ');

        if (CurrentPercent.IsInRange)
        {
            _bar[_leftIndex] = _progressSymbol;
            _bar[_rightIndex] = _progressSymbol;
        }
    }
}
