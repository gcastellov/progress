namespace Progress.Samples;

public class InstallerWorker
{
    public class CalcRequirements
    {
        public const string Name = "Calculate";
        public const string Description = "Calculating physical requirements";
        public const long ExpectedItems = 3;
    }

    public class DonwloadArtifacts
    {
        public const string Name = "Download";
        public const string Description = "Downloading artifacts";
        public const long ExpectedItems = 15;
    }

    public class InstallArtifacts
    {
        public const string Name = "Install";
        public const string Description = "Installing artifacts";
        public const long ExpectedItems = 15000;
    }

    public Action<string> OnSuccess { get; set; } = default!;
    public Action<string> OnFailure { get; set; } = default!;

    public async Task CalcAsync()
    {
        for (long i = 0; i < CalcRequirements.ExpectedItems; i++)
        {
            OnSuccess(CalcRequirements.Name);
            await Task.Delay(TimeSpan.FromSeconds(2));
        }
    }

    public async Task DownloadAsync()
    {
        Random rnd = Random.Shared;

        for (long i = 0; i < DonwloadArtifacts.ExpectedItems; i++)
        {
            OnSuccess(DonwloadArtifacts.Name);
            double nextSecond = rnd.NextDouble();
            await Task.Delay(TimeSpan.FromMilliseconds(nextSecond * 1000));
        }
    }

    public async Task InstallAsync()
    {
        const long BatchSize = 500;

        long rem = InstallArtifacts.ExpectedItems % BatchSize;
        int batchCount = (int)(InstallArtifacts.ExpectedItems / BatchSize);
        if (rem > 0)
            batchCount++;

        string[][] batches = Enumerable.Range(0, batchCount)
            .Select(i =>
            {
                if (rem > 0)
                {
                    return i < batchCount - 1
                        ? new string[BatchSize]
                        : new string[InstallArtifacts.ExpectedItems % BatchSize];
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
                    OnSuccess?.Invoke(InstallArtifacts.Name);
                else
                    OnFailure?.Invoke(InstallArtifacts.Name);
            }
        });
    }
}
