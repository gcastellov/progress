using System.Globalization;

namespace Progress.Components;

internal abstract class Component
{
    private static readonly CultureInfo Culture = new("en-US");

    public abstract Component Next(ulong availableItems, ulong currentCount);

    protected static Percent Calculate(ulong availableItems, ulong currentCount) => new(availableItems, currentCount);

    internal class Percent
    {
        private readonly decimal _percent;
        
        public Percent(ulong items, ulong count)
        {
            if (count > 0)
                _percent = count / (decimal)items;
        }

        public decimal Value => _percent * 100;

        public override string ToString() => _percent.ToString("P02", Culture);
    }
}