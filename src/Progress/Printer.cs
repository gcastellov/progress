using Progress.Reporters;
using Progress.Settings.Console;
using System.Text;

namespace Progress;

internal class Printer(ReportingOptions options, ICollection<Workload> workloads)
{
    private const int Left_Padding = 30;
    private const int Right_Padding = 20;

    private readonly ReportingOptions _options = options; 
    private readonly IEnumerable<Workload> _workloads = workloads;
    
    public string Print(Stats stats)
    {
        StringBuilder sBuilder = new();

        if (_options.DisplayStartingTime)
        {
            string label = "Process started at:".PadRight(Right_Padding);
            sBuilder.AppendLine($"{label} {stats.StartedOn.ToString().PadLeft(Left_Padding)}");
        }

        if (_options.DisplayEstimatedTimeOfArrival)
        {
            string label = "ETA:".PadRight(Right_Padding);
            string value = stats.CurrentPercent == 0
                ? Timer.Unknowm
                : stats.EstTimeOfArrival.ToString();

            sBuilder.AppendLine($"{label} {value.PadLeft(Left_Padding)}");
        }

        if (_options.DisplayElapsedTime)
        {
            string label = "Elapsed time:".PadRight(Right_Padding);
            sBuilder.AppendLine($"{label} {stats.ElapsedTime.ToString().PadLeft(Left_Padding)}");
        }

        if (_options.DisplayRemainingTime)
        {
            string label = "Remaining time:".PadRight(Right_Padding);
            string value = stats.CurrentPercent == 0
                ? Timer.Unknowm
                : stats.RemainingTime.ToString();

            sBuilder.AppendLine($"{label} {value.PadLeft(Left_Padding)}");
        }

        if (_options.DisplayItemsSummary)
        {
            string successLabel = "Successful items:".PadRight(Right_Padding);
            string unsuccessLabel = "Unsuccessful items:".PadRight(Right_Padding);
            sBuilder.AppendLine($"{successLabel} {stats.SuccessCount.ToString().PadLeft(Left_Padding)}");
            sBuilder.AppendLine($"{unsuccessLabel} {stats.FailureCount.ToString().PadLeft(Left_Padding)}");
        }

        if (_options.DisplayItemsOverview)
        {
            string label = "Total items:".PadRight(Right_Padding);
            sBuilder.AppendLine($"{label} {stats.CurrentCount.ToString().PadLeft(Left_Padding)}");
        }

        sBuilder.AppendLine();

        foreach(var workload in _workloads)
        {
            if (_options.HideWorkflowOnComplete && workload.IsFinished)
                continue;

            sBuilder.AppendLine(workload.Description);
            sBuilder.AppendLine(workload.Component.ToString());
            sBuilder.AppendLine();
        }

        return sBuilder.ToString();
    }
}
