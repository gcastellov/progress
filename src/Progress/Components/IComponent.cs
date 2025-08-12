namespace Progress.Components;

public interface IComponent
{
    IComponent Next(ulong availableItems, ulong currentCount);
}
