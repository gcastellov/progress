using Progress.Components;
using System.Text;

namespace Progress;

public class Reporter : IDisposable
{
    private const int LEFT_PADDING = 30;
    private const int RIGHT_PADDING = 20;

    private readonly ulong _itemsCount;
    private IComponent _component;
    private ulong _successCount;
    private ulong _failureCount;
    private DateTimeOffset _startedOn = default!;
    private Thread _reportingThread = default!;

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

    internal Reporter(ulong itemsCount, IComponent component)
    {
        _itemsCount = itemsCount;
        _component = component;
    }

    public void ReportSuccess() => Interlocked.Increment(ref _successCount);
    public void ReportFailure() => Interlocked.Increment(ref _failureCount);

    public void Start()
    {
        _startedOn = DateTimeOffset.UtcNow;

        var tStart = new ThreadStart(() =>
        {
            do
            {
                Display();
                Thread.Sleep(ReportFrequency);
            }
            while (!IsFinished);

            Display();
        });

        _reportingThread = new(tStart);
        _reportingThread.Start();
    }

    public void Display()
    {
        _component = _component.Next(_itemsCount, CurrentCount);
        Console.SetCursorPosition(0, 0);
        byte[] output = Encoding.Default.GetBytes(ToString());
        using var stream = Console.OpenStandardOutput();
        stream.Write(output, 0, output.Length);
        stream.Flush();
    }

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

    public void Dispose()
    {
        if (_reportingThread != null && _reportingThread.IsAlive)
            _reportingThread.Join();
    }
}
