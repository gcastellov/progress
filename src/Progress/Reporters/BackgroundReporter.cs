using Progress.Components;
using Progress.Settings.Background;

namespace Progress.Reporters;

/// <summary>
/// Reports background task's progress based on the settings provided.
/// </summary>
public class BackgroundReporter
{
    private readonly ulong _itemsCount;
    private readonly Configuration _configuration;

    private ulong _successCount;
    private ulong _failureCount;
    private Timer _timer;
    private Thread _statsThread = default!;
    private CancellationTokenSource _cancellationTokenSource = default!;
    private CancellationToken _cancellationToken;
    private DateTimeOffset _lastStatsNotification;

    /// <summary>
    /// Inidicates whehter the task is completed or not.
    /// </summary>
    public bool IsFinished => CurrentCount == _itemsCount;

    internal Configuration Configuration => _configuration;
    internal Action<Stats>? OnProgress { get; set; } = null!;
    internal Action<Stats>? OnCompletion { get; set; } = null!;

    private ulong CurrentCount => _successCount + _failureCount;

    internal BackgroundReporter(ulong itemsCount)
    {
        if (itemsCount == 0)
            throw new ArgumentException($"Nothing to do!. {nameof(itemsCount)} must be greater than 0");

        _itemsCount = itemsCount;
        _timer = Timer.Start();
        _configuration = new Configuration();
    }

    /// <summary>
    /// Reportes success. By calling this method, the success counter increases.
    /// </summary>
    public void ReportSuccess() => Interlocked.Increment(ref _successCount);

    /// <summary>
    /// Reports failures. By calling this method, the failure counter increases.
    /// </summary>
    public void ReportFailure() => Interlocked.Increment(ref _failureCount);

    /// <summary>
    /// Starts a thread that reports the progression of the operation.
    /// </summary>
    public void Start()
    {
        _successCount = 0;
        _failureCount = 0;
        _lastStatsNotification = DateTimeOffset.UtcNow;
        _timer = Timer.Start();
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationToken = _cancellationTokenSource.Token;
        _statsThread = DoStats();
        _statsThread.Start();
    }

    /// <summary>
    /// Stops the thread that reports the progression of the operation.
    /// </summary>
    public void Stop()
    {
        if (_statsThread == null)
            throw new InvalidOperationException($"First start the reporting by calling {nameof(Start)}");

        if (_cancellationToken.CanBeCanceled)
            _cancellationTokenSource.Cancel();

        if (_statsThread != null && _statsThread.IsAlive)
            _statsThread.Join();
    }

    private Thread DoStats()
    {
        var tStart = new ThreadStart(() =>
        {
            do
            {
                ReportStats();
                Thread.Sleep(Configuration.StatsFrequency);
            }
            while (!IsFinished && !_cancellationToken.IsCancellationRequested);

            if (IsFinished)
            {
                ReportCompletionStats();
                Export();
            }
        });

        return new(tStart);
    }

    private void ReportStats()
    {
        if (OnProgress == null || DateTimeOffset.UtcNow - _lastStatsNotification < Configuration.StatsFrequency)
            return;

        var stats = CollectStats();
        OnProgress.Invoke(stats);
        _lastStatsNotification = DateTimeOffset.UtcNow;
    }

    private void ReportCompletionStats()
    {
        if (OnCompletion == null)
            return;

        var stats = CollectStats();
        OnCompletion.Invoke(stats);
    }

    private void Export()
    {
        if (Configuration.Options.ExportCompletionStats && Configuration.ExportSettings != null)
        {
            var stats = CollectStats();
            new Exporter(Configuration.ExportSettings).Export(stats);
        }
    } 

    private Stats CollectStats()
    {
        var percent = Component.Calculate(_itemsCount, CurrentCount);

        return new()
        {
            StartedOn = _timer.StartedOn,
            ElapsedTime = _timer.ElapsedTime,
            RemainingTime = _timer.GetRemainingTime(percent.Value),
            EstTimeOfArrival = _timer.GetEstimatedTimeOfArrival(percent.Value),
            ExpectedItems = _itemsCount,
            CurrentCount = CurrentCount,
            SuccessCount = _successCount,
            FailureCount = _failureCount,
            CurrentPercent = percent.Value,
        };
    }
}
