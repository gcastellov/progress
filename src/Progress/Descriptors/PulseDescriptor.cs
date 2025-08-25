using Progress.Components;

namespace Progress.Descriptors;

/// <summary>
/// This <see cref="ComponentDescriptor"/> is used to create a <see cref="PulseDescriptor"/> component.
/// </summary>
public class PulseDescriptor : ComponentDescriptor
{
    private char _progressSymbol = (char)35;
    private uint _width = 100;

    /// <summary>
    /// Helper to get a default <see cref="PulseDescriptor"/>.
    /// </summary>
    public static PulseDescriptor Default =>
        new PulseDescriptor()
            .DisplayingPercent()
            .UsingProgressSymbol((char)35)
            .UsingWidth(40);

    /// <summary>
    /// Sets the width of the bar.
    /// </summary>
    /// <param name="width"></param>
    /// <returns></returns>
    public PulseDescriptor UsingWidth(uint width)
    {
        _width = width;
        return this;
    }

    /// <summary>
    /// Sets the char to represent the progression.
    /// </summary>
    /// <param name="progressSymbol"></param>
    /// <returns></returns>
    public PulseDescriptor UsingProgressSymbol(char progressSymbol)
    {
        _progressSymbol = progressSymbol;
        return this;
    }

    internal new PulseDescriptor DisplayingPercent() => (PulseDescriptor)base.DisplayingPercent();

    internal override Component Build()
    {
        return new Pulse(_width, _progressSymbol)
        {
            DisplayPercent = DisplayPercent
        };
    }
}
