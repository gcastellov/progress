namespace Progress.Samples.Utils;

public static class ConsoleUtils
{
    public static bool UseAggregateReporter(string[] args)
    {
        if (args.Length < 2)
            return false;

        return args[0] == "--type" && args[1] == "aggregate";
    }
}
