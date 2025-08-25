using System.Text;

namespace Progress.Components;

internal class Pulse : Component
{
    private readonly char _progressSymbol;
    private readonly char[] _bar;
    
    private uint _index;
    private Percent _percent = default!;

    public Pulse(uint width, char progressSymbol)
    {
        _progressSymbol = progressSymbol;
        _bar = new char[width];
    }

    public bool DisplayPercent { get; init; } = true;


    public override Component Next(ulong availableItems, ulong currentCount)
    {
        _percent = Calculate(availableItems, currentCount);

        _index++;

        if (_index == _bar.Length)
            _index = 0;

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
            sBuilder.Append(_percent.ToString());
        }

        return sBuilder.ToString();
    }

    private void Fill()
    {
        Array.Fill(_bar, ' ');

        if (_percent.Value < 100)
            _bar[_index] = _progressSymbol;
    }
}
