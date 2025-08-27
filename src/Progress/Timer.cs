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
        var eta = GetEstimatedTimeOfArrival(currentPercent);
        var remaining = eta - DateTimeOffset.Now;

        if (remaining < TimeSpan.Zero)
            return TimeSpan.Zero;

        return remaining;
    }

    public DateTimeOffset GetEstimatedTimeOfArrival(double currentPercent)
    {
        double ms = ElapsedTime.TotalMilliseconds / currentPercent * 100;
        return _startedOn.AddMilliseconds(ms);
    }
}
