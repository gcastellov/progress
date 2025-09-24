namespace Progress.Samples;

public class SimpleWorker
{
    public const long ExpectedItems = 85161;
    private const long BatchSize = 500;

    public Action OnSuccess { get; set; } = default!;
    public Action OnFailure { get; set; } = default!;

    public async Task DoMyworkAsync()
    {
        long rem = ExpectedItems % BatchSize;
        int batchCount = (int)(ExpectedItems / BatchSize);
        if (rem > 0)
            batchCount++;

        string[][] batches = Enumerable.Range(0, batchCount)
            .Select(i =>
            {
                if (rem > 0)
                {
                    return i < batchCount - 1
                        ? new string[BatchSize]
                        : new string[ExpectedItems % BatchSize];
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
                    OnSuccess?.Invoke();
                else
                    OnFailure?.Invoke();
            }
        });
    }
}
