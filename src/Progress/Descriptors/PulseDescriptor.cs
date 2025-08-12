using Progress.Components;

namespace Progress.Descriptors;

public class PulseDescriptor : ComponentDescriptor
{
    private char _progressSymbol = (char)35;
    private uint _width = 100;

    public static PulseDescriptor Default =>
        new PulseDescriptor()
            .DisplayingPercent()
            .UsingProgressSymbol((char)35)
            .UsingWidth(40);

    public new PulseDescriptor DisplayingPercent() => (PulseDescriptor)base.DisplayingPercent();

    public PulseDescriptor UsingWidth(uint width)
    {
        _width = width;
        return this;
    }

    public PulseDescriptor UsingProgressSymbol(char progressSymbol)
    {
        _progressSymbol = progressSymbol;
        return this;
    }

    internal override IComponent Build(ulong availableItems)
    {
        return new Pulse(_width, _progressSymbol)
        {
            DisplayPercent = DisplayPercent
        };
    }
}
