using Progress.Components;

namespace Progress.Descriptors;

public class HearthBeatDescriptor : ComponentDescriptor
{
    private char _progressSymbol = (char)35;
    private uint _width = 100;

    public static HearthBeatDescriptor Default =>
        new HearthBeatDescriptor()
            .DisplayingPercent()
            .UsingProgressSymbol((char)35)
            .UsingWidth(40);

    public new HearthBeatDescriptor DisplayingPercent() => (HearthBeatDescriptor)base.DisplayingPercent();

    public HearthBeatDescriptor UsingWidth(uint width)
    {
        _width = width;
        return this;
    }

    public HearthBeatDescriptor UsingProgressSymbol(char progressSymbol)
    {
        _progressSymbol = progressSymbol;
        return this;
    }

    internal override IComponent Build(ulong availableItems)
    {
        return new HearthBeat(_width, _progressSymbol)
        {
            DisplayPercent = DisplayPercent
        };
    }
}
