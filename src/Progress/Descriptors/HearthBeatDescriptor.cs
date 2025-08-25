using Progress.Components;

namespace Progress.Descriptors;

/// <summary>
/// This <see cref="ComponentDescriptor"/> is used to create a <see cref="HearthBeat"/> component.
/// </summary>
public class HearthBeatDescriptor : ComponentDescriptor
{
    private char _progressSymbol = (char)35;
    private uint _width = 100;

    /// <summary>
    /// Helper to get a default <see cref="HearthBeatDescriptor"/>.
    /// </summary>
    public static HearthBeatDescriptor Default =>
        new HearthBeatDescriptor()
            .DisplayingPercent()
            .UsingProgressSymbol((char)35)
            .UsingWidth(40);

    /// <summary>
    /// Sets the width of the bar.
    /// </summary>
    /// <param name="width"></param>
    /// <returns></returns>
    public HearthBeatDescriptor UsingWidth(uint width)
    {
        _width = width;
        return this;
    }

    /// <summary>
    /// Sets the char to represent the progression.
    /// </summary>
    /// <param name="progressSymbol"></param>
    /// <returns></returns>
    public HearthBeatDescriptor UsingProgressSymbol(char progressSymbol)
    {
        _progressSymbol = progressSymbol;
        return this;
    }

    internal new HearthBeatDescriptor DisplayingPercent() => (HearthBeatDescriptor)base.DisplayingPercent();

    internal override Component Build()
    {
        return new HearthBeat(_width, _progressSymbol)
        {
            DisplayPercent = DisplayPercent
        };
    }
}
