using Progress.Reporters;

namespace Progress.Samples;

public class InstallerWorker
{
    public Action<string> OnSuccess { get; set; } = default!;
    public Action<string> OnFailure { get; set; } = default!;

    public CalcRequirementsWorkload CalcRequirements { get; }
    public DownloadArtifactsWorkload DownloadArtifacts { get; }
    public InstallArtifactsWorkload InstallArtifacts { get; }

    public InstallerWorker()
    {
        CalcRequirements = new(this);
        DownloadArtifacts = new(this);
        InstallArtifacts = new(this);
    }

    public class CalcRequirementsWorkload(InstallerWorker worker) : Workload("Calculate", "Calculating physical requirements", 3)
    {
        public async Task CalcAsync()
        {
            for (ulong i = 0; i < ItemsCount; i++)
            {
                worker.OnSuccess(Id);
                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }
    }

    public class DownloadArtifactsWorkload(InstallerWorker worker) : Workload("Download", "Downloading artifacts", 15)
    {
        public async Task DownloadAsync()
        {
            Random rnd = Random.Shared;

            for (ulong i = 0; i < ItemsCount; i++)
            {
                worker.OnSuccess(Id);
                double nextSecond = rnd.NextDouble();
                await Task.Delay(TimeSpan.FromMilliseconds(nextSecond * 1000));
            }
        }
    }

    public class InstallArtifactsWorkload(InstallerWorker worker) : Workload("Install", "Installing artifacts", 15000)
    {
        public async Task InstallAsync()
        {
            const long BatchSize = 500;

            ulong rem = ItemsCount % BatchSize;
            int batchCount = (int)(ItemsCount / BatchSize);
            if (rem > 0)
                batchCount++;

            string[][] batches = Enumerable.Range(0, batchCount)
                .Select(i =>
                {
                    if (rem > 0)
                    {
                        return i < batchCount - 1
                            ? new string[BatchSize]
                            : new string[ItemsCount % BatchSize];
                    }

                    return new string[BatchSize];
                })
                .ToArray();

            await Parallel.ForEachAsync(batches, async (items, ct) =>
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if (i % 3 == 0)
                        await Task.Delay(TimeSpan.FromMilliseconds(10));

                    if (i % 7 == 0)
                        worker.OnSuccess?.Invoke(Id);
                    else
                        worker.OnFailure?.Invoke(Id);
                }
            });
        }
    }
}
