using System.Text;

namespace Progress.Components;

internal class Spinner : Component
{
    private int _counter;
    private char _current;

    public bool DisplayPercent { get; init; } = true;

    public override Component Next(ulong availableItems, ulong currentCount)
    {
        CurrentPercent = Calculate(availableItems, currentCount);
        return this;
    }

    public override string ToString()
    {
        Fill();

        var sBuilder = new StringBuilder();
        sBuilder.Append(_current);

        if (DisplayPercent)
        {
            sBuilder.Append(' ');
            sBuilder.Append(CurrentPercent.ToString());
        }

        return sBuilder.ToString();
    }

    private void Fill()
    {
        if (CurrentPercent.IsInRange)
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
        }
        else
        {
            _current = ' ';
        }
    }
}
