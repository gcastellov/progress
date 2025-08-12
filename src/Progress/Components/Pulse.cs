using System.Text;

namespace Progress.Components;

internal class Pulse : IComponent
{
    private readonly char _progressSymbol;
    private readonly char[] _bar;
    
    private uint _index;
    private decimal _percent;
    private Pulse _pulse = default!;

    public Pulse(uint width, char progressSymbol)
    {
        _progressSymbol = progressSymbol;
        _bar = new char[width];
    }

    public bool DisplayPercent { get; set; } = true;


    public IComponent Next(ulong availableItems, ulong currentCount)
    {
        _percent = (decimal)currentCount / (decimal)availableItems * 100;

        _index++;

        if (_index == _bar.Length)
            _index = 0;

        return _pulse ??= this;
    }

    public override string ToString()
    {
        Array.Fill(_bar, ' ');

        if (_percent < 100)
            _bar[_index] = _progressSymbol;

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
}
