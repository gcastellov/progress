namespace Progress;

internal class Timer(DateTimeOffset startedOn)
{
    public const string Unknowm = "Unknowm";

    private readonly DateTimeOffset _startedOn = startedOn;

    public TimeSpan ElapsedTime => DateTimeOffset.UtcNow - _startedOn;
    public DateTimeOffset StartedOn => _startedOn;

    public static Timer Start() => new(DateTimeOffset.UtcNow);

    public TimeSpan GetRemainingTime(double currentPercent)
    {
        if (currentPercent == 0)
            return TimeSpan.MaxValue;

        var eta = GetEstimatedTimeOfArrival(currentPercent);
        var remaining = eta - DateTimeOffset.Now;

        if (remaining < TimeSpan.Zero)
            return TimeSpan.Zero;

        return remaining;
    }

    public DateTimeOffset GetEstimatedTimeOfArrival(double currentPercent)
    {
        if (currentPercent == 0)
            return DateTimeOffset.MaxValue;

        double ms = ElapsedTime.TotalMilliseconds / currentPercent * 100;
        return _startedOn.AddMilliseconds(ms);
    }
}
