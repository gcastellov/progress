using Progress.Components;

namespace Progress.Descriptors;

public class BarDescriptor : ComponentDescriptor
{
    private char _progressSymbol = (char)35;
    private uint _width = 100;

    public static BarDescriptor Default =>
        new BarDescriptor()
            .DisplayingPercent()
            .UsingProgressSymbol((char)35)
            .UsingWidth(40);

    public new BarDescriptor DisplayingPercent() => (BarDescriptor)base.DisplayingPercent();

    public BarDescriptor UsingWidth(uint width)
    {
        _width = width;
        return this;
    }

    public BarDescriptor UsingProgressSymbol(char progressSymbol)
    {
        _progressSymbol = progressSymbol;
        return this;
    }

    internal override IComponent Build(ulong availableItems)
    {
        return new Bar(availableItems, 0, _width, _progressSymbol)
        {
            DisplayPercent = DisplayPercent
        };
    }
}
