using Progress.Components;
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

    private Component _component;
    private ulong _successCount;
    private ulong _failureCount;
    private DateTimeOffset _startedOn = default!;
    private Thread _reportingThread = default!;
    private CancellationTokenSource _cancellationTokenSource = default!;
    private CancellationToken _cancellationToken;

    /// <summary>
    /// Inidicates whehter the task is completed or not.
    /// </summary>
    public bool IsFinished => CurrentCount == _itemsCount;
    
    internal bool DisplayElapsedTime { get; set; } = true;
    internal bool DisplayStartingTime { get; set; } = true;
    internal bool DisplayItemsOverview { get; set; } = true;
    internal bool DisplayItemsSummary { get; set; } = true;
    internal TimeSpan ReportFrequency { get; set; } = TimeSpan.FromSeconds(1);
    
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
        _startedOn = DateTimeOffset.UtcNow;
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationToken = _cancellationTokenSource.Token;
        _reportingThread = DoWork();
        _reportingThread.Start();
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

        _reportingThread = DoWork();
        _reportingThread.Start();
    }

    /// <summary>
    /// Gets the reporter's status.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        var elapsedTime = DateTimeOffset.UtcNow - _startedOn;

        StringBuilder sBuilder = new();

        if (DisplayStartingTime)
        {
            string label = "Process started at:".PadRight(RIGHT_PADDING);
            sBuilder.AppendLine($"{label} {_startedOn.ToString().PadLeft(LEFT_PADDING)}");

        }

        if (DisplayElapsedTime)
        {
            string label = "Elapsed time:".PadRight(RIGHT_PADDING);
            sBuilder.AppendLine($"{label} {elapsedTime.ToString().PadLeft(LEFT_PADDING)}");
        }

        if (DisplayItemsSummary)
        {
            string successLabel = "Successful items:".PadRight(RIGHT_PADDING);
            string unsuccessLabel = "Unsuccessful items:".PadRight(RIGHT_PADDING);
            sBuilder.AppendLine($"{successLabel} {SuccessfulItems.ToString().PadLeft(LEFT_PADDING)}");
            sBuilder.AppendLine($"{unsuccessLabel} {UnsuccessfulItems.ToString().PadLeft(LEFT_PADDING)}");
        }

        if (DisplayItemsOverview)
        {
            string label = "Total items:".PadRight(RIGHT_PADDING);
            sBuilder.AppendLine($"{label} {AllItems.ToString().PadLeft(LEFT_PADDING)}");
        }

        sBuilder.AppendLine();
        sBuilder.AppendLine(_component.ToString());            
        return sBuilder.ToString();
    }

    /// <summary>
    /// Cancels the reporting task and waits for the reporting thread in case it is alive
    /// </summary>
    public void Dispose()
    {
        if (_cancellationToken.CanBeCanceled)
            _cancellationTokenSource.Cancel();

        if (_reportingThread != null && _reportingThread.IsAlive)
            _reportingThread.Join();
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

    private Thread DoWork()
    {
        var tStart = new ThreadStart(() =>
        {
            do
            {
                Display();
                Thread.Sleep(ReportFrequency);
            }
            while (!IsFinished && !_cancellationToken.IsCancellationRequested);

            Display();
        });

        return new(tStart);
    }
}
