using Progress.Components;

namespace Progress.Descriptors;

/// <summary>
/// This <see cref="ComponentDescriptor"/> is used to create a <see cref="Bar"/> component.
/// </summary>
public class BarDescriptor : ComponentDescriptor
{
    private char _progressSymbol = (char)35;
    private uint _width = 100;

    /// <summary>
    /// Helper to get a default <see cref="BarDescriptor"/>.
    /// </summary>
    public static BarDescriptor Default =>
        new BarDescriptor()
            .DisplayingPercent()
            .UsingProgressSymbol((char)35)
            .UsingWidth(40);

    /// <summary>
    /// Sets the width of the progress bar.
    /// </summary>
    /// <param name="width"></param>
    /// <returns></returns>
    public BarDescriptor UsingWidth(uint width)
    {
        _width = width;
        return this;
    }

    /// <summary>
    /// Sets the char to represent the progression.
    /// </summary>
    /// <param name="progressSymbol"></param>
    /// <returns></returns>
    public BarDescriptor UsingProgressSymbol(char progressSymbol)
    {
        _progressSymbol = progressSymbol;
        return this;
    }

    internal new BarDescriptor DisplayingPercent() => (BarDescriptor)base.DisplayingPercent();


    internal override Component Build()
    {
        return new Bar(_width, _progressSymbol)
        {
            DisplayPercent = DisplayPercent
        };
    }
}
