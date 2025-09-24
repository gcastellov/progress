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
    var worker = new InstallerWorker();
    using var reporter = ReporterFactory.GetConsoleAggregateReporterBuilder(worker);
    worker.OnSuccess = (name) => reporter.ReportSuccess(name);
    worker.OnFailure = (name) => reporter.ReportFailure(name);

    reporter.Start();
    await worker.CalcRequirements.CalcAsync();
    Task[] restTasks = [worker.DownloadArtifacts.DownloadAsync(), worker.InstallArtifacts.InstallAsync()];
    await Task.WhenAll(restTasks);
};

//await executeInstallerSample();
await executeSimpleSample();
