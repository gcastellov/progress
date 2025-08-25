using Progress.Components;

namespace Progress.Descriptors;

/// <summary>
/// This <see cref="ComponentDescriptor"/> is used to create a <see cref="SpinnerDescriptor"/> component.
/// </summary>
public class SpinnerDescriptor : ComponentDescriptor
{
    /// <summary>
    /// Helper to get a default <see cref="SpinnerDescriptor"/>.
    /// </summary>
    public static SpinnerDescriptor Default => new SpinnerDescriptor().DisplayingPercent();

    internal new SpinnerDescriptor DisplayingPercent() => (SpinnerDescriptor)base.DisplayingPercent();

    internal override Component Build()
    {
        return new Spinner() { DisplayPercent = DisplayPercent };
    }
}
