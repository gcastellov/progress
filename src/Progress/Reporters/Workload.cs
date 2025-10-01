using Progress.Components;
using System.Collections;

namespace Progress.Reporters
{
    /// <summary>
    /// Initializes a new instance of <see cref="Workload"/> describing the task to perform.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="description"></param>
    /// <param name="expectedItems"></param>
    public class Workload(string id, string description, ulong expectedItems)
    {
        /// <summary>
        /// Helper to initialize a new instance of <see cref="Workload"/> with its identifier as 'Default'
        /// </summary>
        /// <param name="itemsCount"></param>
        /// <returns></returns>
        public static Workload Default(ulong itemsCount) => new("Default", string.Empty, itemsCount);

        private ulong _successCount;
        private ulong _failureCount;

        /// <summary>
        /// Gets the workload's identifier
        /// </summary>
        public string Id { get; } = id;
        
        /// <summary>
        /// Gets the workflow description 
        /// </summary>
        public string Description { get; } = description;
        
        /// <summary>
        /// Gets the exepected items count
        /// </summary>
        public ulong ItemsCount { get; } = expectedItems;

        internal Component Component { get; set; } = default!;
        internal ulong SuccessCount => _successCount;
        internal ulong FailureCount => _failureCount;
        internal ulong CurrentCount => SuccessCount + FailureCount;
        internal bool IsFinished => CurrentCount == ItemsCount;

        internal void ReportSuccess() => Interlocked.Increment(ref _successCount);
        internal void ReportFailure() => Interlocked.Increment(ref _failureCount);
        internal void Next() => Component.Next(ItemsCount, CurrentCount);

        internal void Reset()
        {
            _successCount = 0;
            _failureCount = 0;
        }
    }

    internal class Workloads(ICollection<Workload> workloads) : IEnumerable<Workload>
    {
        private readonly Dictionary<string, Workload> _workloads = workloads.ToDictionary(w => w.Id, w => w);

        public IEnumerator<Workload> GetEnumerator() => _workloads.Values.GetEnumerator();
        public double CurrentPercent => _workloads.Values.Select(w => w.Component.CurrentPercent.Value).Sum() / _workloads.Count();
        public ulong AllItemsCount { get; } = (ulong)Enumerable.Sum(workloads, (w) => (long)w.ItemsCount);
        public ulong CurrentCount => (ulong)Enumerable.Sum(_workloads.Values, (w) => (long)w.CurrentCount);
        public ulong AllSuccess => (ulong)Enumerable.Sum(_workloads.Values, (w) => (long)w.SuccessCount);
        public ulong AllFailures => (ulong)Enumerable.Sum(_workloads.Values, (w) => (long)w.FailureCount);
        public bool AreFinished => _workloads.Values.All(w => w.IsFinished);

        public void Next()
        {
            foreach (var workload in _workloads.Values)
                workload.Next();
        }

        public void Success(string workloadName)
        { 
            if (_workloads.ContainsKey(workloadName))
                _workloads[workloadName].ReportSuccess();
        }

        public void Failure(string workloadName)
        {
            if (_workloads.ContainsKey(workloadName))
                _workloads[workloadName].ReportFailure();
        }

        public Dictionary<string, bool> GetOverview() => _workloads.ToDictionary(w => w.Key, w => w.Value.IsFinished);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
