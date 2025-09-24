using Progress.Settings.Console;
using System.Text;

namespace Progress.Reporters;

/// <summary>
/// Base class for console based reporters.
/// </summary>
public abstract class ConsoleReporterBase : IDisposable
{
    private readonly Configuration _configuration;
    private readonly Workloads _workloads;

    private bool _isDisposed;
    private Timer _timer;
    private Printer _printer;
    private Thread _reportingThread = default!;
    private Thread _statsThread = default!;
    private CancellationTokenSource _cancellationTokenSource = default!;
    private CancellationToken _cancellationToken;
    private DateTimeOffset _lastStatsNotification;
    private Dictionary<string, bool> _lastWorkflowOverview;

    /// <summary>
    /// Inidicates whehter the task is completed or not.
    /// </summary>
    public bool IsFinished => _workloads.AreFinished;

    internal Configuration Configuration => _configuration;
    internal Action<Stats>? OnProgress { get; set; } = null!;
    internal Action<Stats>? OnCompletion { get; set; } = null!;
    internal Timer Timer => _timer;
    internal Printer Printer => _printer;
    internal Workloads Workloads => _workloads;

    /// <summary>
    /// Initializes an insatance of any console based reporter.
    /// </summary>
    /// <param name="workloads"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    internal ConsoleReporterBase(ICollection<Workload> workloads)
    {
        foreach (var workload in workloads)
        {
            if (workload.ItemsCount == 0)
                throw new ArgumentException($"Nothing to do!. {nameof(workload.ItemsCount)} must be greater than 0");

            if (workload.Component == null)
                throw new ArgumentNullException($"{nameof(workload.Component)} cannot be null");
        }

        _timer = Timer.Start();
        _configuration = new Configuration();
        _workloads = new Workloads(workloads);
        _printer = new Printer(_configuration.Options, _workloads.ToArray());
        _lastWorkflowOverview = _workloads.GetOverview();
    }

    /// <summary>
    /// Starts a thread that refeshes the console output displaying the progress of the operation.
    /// </summary>
    public void Start()
    {
        if (_isDisposed)
            throw new ObjectDisposedException("Instance already disposed");

        foreach (var workload in _workloads)
            workload.Reset();

        _lastStatsNotification = DateTimeOffset.UtcNow;
        _timer = Timer.Start();
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationToken = _cancellationTokenSource.Token;
        _reportingThread = DoWork();
        _reportingThread.Start();

        if (_configuration.Options.NotifyProgressStats)
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

        if (_configuration.Options.NotifyProgressStats)
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
        var stats = CollectStats();
        return _printer.Print(stats);
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

                OnProgress = null!;
                OnCompletion = null!;
            }
        }
    }

    /// <summary>
    /// Collect stats relative to all workloads
    /// </summary>
    /// <returns></returns>
    protected abstract Stats CollectStats();

    private Thread DoWork()
    {
        var tStart = new ThreadStart(() =>
        {
            do
            {
                Display();
                Thread.Sleep(_configuration.ReportFrequency);
            }
            while (!IsFinished && !_cancellationToken.IsCancellationRequested);

            Display();

            if (IsFinished && (_configuration.Options.NotifyCompletionStats || _configuration.Options.ExportCompletionStats))
            {
                var stats = CollectStats();
                OnCompletion?.Invoke(stats);

                if (_configuration.ExportSettings != null)
                    new Exporter(_configuration.ExportSettings).Export(stats);
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
                Thread.Sleep(_configuration.StatsFrequency);
            }
            while (!IsFinished && !_cancellationToken.IsCancellationRequested);
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

    private void Display()
    {
        _workloads.Next();
        var currentWorflowOverview = _workloads.GetOverview();

        if (!Console.IsOutputRedirected)
        {
            if (_configuration.Options.HideWorkflowOnComplete && _lastWorkflowOverview.Except(currentWorflowOverview).Any())
                Console.Clear();

            Console.SetCursorPosition(0, 0);
        }

        _lastWorkflowOverview = currentWorflowOverview;
        byte[] output = Encoding.Default.GetBytes(ToString());
        using var stream = Console.OpenStandardOutput();
        stream.Write(output, 0, output.Length);
        stream.Flush();

    }
}
