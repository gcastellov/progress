using Progress.Components;
using Progress.Settings;
using System.Text;

namespace Progress;

/// <summary>
/// Reports background task completion based on the settings provided.
/// </summary>
public class Reporter : IDisposable
{
    private const int LEFT_PADDING = 30;
    private const int RIGHT_PADDING = 20;

    private readonly ulong _itemsCount;

    private bool _isDisposed;
    private ulong _successCount;
    private ulong _failureCount;
    private Component _component;
    private Timer _timer;
    private Thread _reportingThread = default!;
    private Thread _statsThread = default!;
    private CancellationTokenSource _cancellationTokenSource = default!;
    private CancellationToken _cancellationToken;
    private DateTimeOffset _lastStatsNotification;

    /// <summary>
    /// Inidicates whehter the task is completed or not.
    /// </summary>
    public bool IsFinished => CurrentCount == _itemsCount;

    internal Configuration Configuration { get; set; } = new();
    internal Action<Stats>? OnProgress { get; set; } = null!;
    internal Action<Stats>? OnCompletion { get; set; } = null!;

    private string AllItems => (_successCount + _failureCount).ToString().PadLeft(10);
    private string SuccessfulItems => _successCount.ToString().PadLeft(10);
    private string UnsuccessfulItems => _failureCount.ToString().PadLeft(10);
    private ulong CurrentCount => _successCount + _failureCount;


    internal Reporter(ulong itemsCount, Component component)
    {
        if (itemsCount == 0)
            throw new ArgumentException($"Nothing to do!. {nameof(itemsCount)} must be greater than 0");

        if (component == null)
            throw new ArgumentNullException($"{nameof(component)} cannot be null");

        _itemsCount = itemsCount;
        _component = component;
        _timer = Timer.Start();
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
    /// Starts a thread that refeshes the console output displaying the progress of the operation.
    /// </summary>
    public void Start()
    {
        if (_isDisposed)
            throw new ObjectDisposedException("Instance already disposed");

        _successCount = 0;
        _failureCount = 0;
        _lastStatsNotification = DateTimeOffset.UtcNow;
        _timer = Timer.Start();
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationToken = _cancellationTokenSource.Token;
        _reportingThread = DoWork();
        _reportingThread.Start();

        if (Configuration.Options.NotifyProgressStats)
        {
            _statsThread = DoStats();
            _statsThread.Start();
        }
    }

    /// <summary>
    /// Stops the thread that refeshes the console output.
    /// </summary>
    public void Stop()
    {
        if (_reportingThread == null)
            throw new InvalidOperationException($"First start the reporting by calling {nameof(Start)}");

        if (_cancellationToken.CanBeCanceled)
            _cancellationTokenSource.Cancel();

        if (_reportingThread != null && _reportingThread.IsAlive)
            _reportingThread.Join();

        if (_statsThread != null && _statsThread.IsAlive)
            _statsThread.Join();
    }

    /// <summary>
    /// Resumes the reporting operation.
    /// </summary>
    public void Resume()
    {
        if (_reportingThread == null)
            throw new InvalidOperationException($"First start the reporting by calling {nameof(Start)}");

        if (_reportingThread.IsAlive)
            throw new InvalidOperationException($"The reporting is still in progress.");

        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationToken = _cancellationTokenSource.Token;
        _reportingThread = DoWork();
        _reportingThread.Start();

        if (Configuration.Options.NotifyProgressStats)
        {
            _statsThread = DoStats();
            _statsThread.Start();
        }
    }

    /// <summary>
    /// Gets the reporter's status.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        StringBuilder sBuilder = new();

        if (Configuration.Options.DisplayStartingTime)
        {
            string label = "Process started at:".PadRight(RIGHT_PADDING);
            sBuilder.AppendLine($"{label} {_timer.StartedOn.ToString().PadLeft(LEFT_PADDING)}");
        }

        if (Configuration.Options.DisplayEstimatedTimeOfArrival)
        {
            string label = "ETA:".PadRight(RIGHT_PADDING);
            string value = _component.CurrentPercent.Value == 0
                ? Timer.Unknowm
                : _timer.GetEstimatedTimeOfArrival(_component.CurrentPercent.Value).ToString();
                
            sBuilder.AppendLine($"{label} {value.PadLeft(LEFT_PADDING)}");
        }

        if (Configuration.Options.DisplayElapsedTime)
        {
            string label = "Elapsed time:".PadRight(RIGHT_PADDING);
            sBuilder.AppendLine($"{label} {_timer.ElapsedTime.ToString().PadLeft(LEFT_PADDING)}");
        }

        if (Configuration.Options.DisplayRemainingTime)
        {
            string label = "Remaining time:".PadRight(RIGHT_PADDING);
            string value = _component.CurrentPercent.Value == 0
                ? Timer.Unknowm
                : _timer.GetRemainingTime(_component.CurrentPercent.Value).ToString();

            sBuilder.AppendLine($"{label} {value.PadLeft(LEFT_PADDING)}");
        }

        if (Configuration.Options.DisplayItemsSummary)
        {
            string successLabel = "Successful items:".PadRight(RIGHT_PADDING);
            string unsuccessLabel = "Unsuccessful items:".PadRight(RIGHT_PADDING);
            sBuilder.AppendLine($"{successLabel} {SuccessfulItems.ToString().PadLeft(LEFT_PADDING)}");
            sBuilder.AppendLine($"{unsuccessLabel} {UnsuccessfulItems.ToString().PadLeft(LEFT_PADDING)}");
        }

        if (Configuration.Options.DisplayItemsOverview)
        {
            string label = "Total items:".PadRight(RIGHT_PADDING);
            sBuilder.AppendLine($"{label} {AllItems.ToString().PadLeft(LEFT_PADDING)}");
        }

        sBuilder.AppendLine();
        sBuilder.AppendLine(_component.ToString());            
        return sBuilder.ToString();
    }

    /// <summary>
    /// Call <see cref="Dispose()"/> and call for finalize
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Cancels the reporting task and waits for the reporting thread in case it is alive.
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            _isDisposed = true;

            if (disposing)
            {
                if (_cancellationToken.CanBeCanceled)
                    _cancellationTokenSource.Cancel();

                if (_reportingThread != null && _reportingThread.IsAlive)
                    _reportingThread.Join();
            }
        }
    }

    private void Display()
    {
        if (!Console.IsOutputRedirected)
            Console.SetCursorPosition(0, 0);
        
        _component = _component.Next(_itemsCount, CurrentCount);
        byte[] output = Encoding.Default.GetBytes(ToString());
        using var stream = Console.OpenStandardOutput();
        stream.Write(output, 0, output.Length);
        stream.Flush();
    }

    private void ReportStats()
    {
        if (OnProgress == null || DateTimeOffset.UtcNow - _lastStatsNotification < Configuration.StatsFrequency)
            return;

        var stats = CollectStats();
        OnProgress.Invoke(stats);
        _lastStatsNotification = DateTimeOffset.UtcNow;
    }

    private Thread DoWork()
    {
        var tStart = new ThreadStart(() =>
        {
            do
            {
                Display();
                Thread.Sleep(Configuration.ReportFrequency);
            }
            while (!IsFinished && !_cancellationToken.IsCancellationRequested);

            Display();

            if (IsFinished && Configuration.Options.NotifyCompletionStats)
            {
                var stats = CollectStats();
                OnCompletion?.Invoke(stats);
            }
        });

        return new(tStart);
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
        });

        return new(tStart);
    }

    private Stats CollectStats()
    {
        return new()
        {
            StartedOn = _timer.StartedOn,
            ElapsedTime = _timer.ElapsedTime,
            RemainingTime = _timer.GetRemainingTime(_component.CurrentPercent.Value),
            EstTimeOfArrival = _timer.GetEstimatedTimeOfArrival(_component.CurrentPercent.Value),
            ExpectedItems = _itemsCount,
            CurrentCount = CurrentCount,
            SuccessCount = _successCount,
            FailureCount = _failureCount,
            CurrentPercent = _component.CurrentPercent.Value,
        };
    }
}
