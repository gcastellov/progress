using Progress.Samples;
using Progress.Samples.Bar.App;


var executeSimpleSample = async () =>
{
    using var reporter = ReporterFactory.GetConsoleReporterBuilder();

    var worker = new SimpleWorker()
    {
        OnSuccess = () => reporter.ReportSuccess(),
        OnFailure = () => reporter.ReportFailure(),
    };

    reporter.Start();
    await worker.DoMyworkAsync();
};

var executeInstallerSample = async () =>
{
    using var reporter = ReporterFactory.GetConsoleAggregateReporterBuilder();

    var worker = new InstallerWorker()
    {
        OnSuccess = (name) => reporter.ReportSuccess(name),
        OnFailure = (name) => reporter.ReportFailure(name),
    };

    reporter.Start();
    await worker.CalcAsync();
    Task[] restTasks = [worker.DownloadAsync(), worker.InstallAsync()];
    await Task.WhenAll(restTasks);
};

await executeInstallerSample();
