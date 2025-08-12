using System.Text;

namespace Progress.Components;

internal class Spinner : IComponent
{
    private int _counter;
    private char _current;
    private decimal _percent;

    private Spinner _spinner = default!;

    public bool DisplayPercent { get; set; } = true;

    public IComponent Next(ulong availableItems, ulong currentCount)
    {
        _percent = (decimal)currentCount / (decimal)availableItems * 100;
        return _spinner ??= this;
    }

    public override string ToString()
    {
        _current = _counter switch
        {
            1 => '/',
            2 => '-',
            3 => '\\',
            5 => '/',
            6 => '-',
            7 => '\\',
            _ => '|'
        };

        if (_counter == 7)
            _counter = 0;
        else
            _counter++;

        var sBuilder = new StringBuilder();
        sBuilder.Append(_current);

        if (DisplayPercent)
        {
            sBuilder.Append(' ');
            sBuilder.Append(_percent.ToString("0.00"));
            sBuilder.Append(" %");
        }

        return sBuilder.ToString();
    }
}
