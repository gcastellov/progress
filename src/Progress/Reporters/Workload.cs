using Progress.Components;
using System.Collections;

namespace Progress.Reporters
{
    internal class Workload(string name, string description, ulong itemsCount, Component component)
    {
        public static Workload Default(ulong itemsCount, Component component) => new("Default", string.Empty, itemsCount, component);

        private ulong _successCount;
        private ulong _failureCount;

        public string Name { get; } = name;
        public string Description { get; } = description;
        public ulong ItemsCount { get; } = itemsCount;
        public Component Component { get; } = component;
        public ulong SuccessCount => _successCount;
        public ulong FailureCount => _failureCount;
        public ulong CurrentCount => SuccessCount + FailureCount;
        public bool IsFinished => CurrentCount == ItemsCount;

        public void ReportSuccess() => Interlocked.Increment(ref _successCount);
        public void ReportFailure() => Interlocked.Increment(ref _failureCount);
        public void Next() => Component.Next(ItemsCount, CurrentCount);

        public void Reset()
        {
            _successCount = 0;
            _failureCount = 0;
        }
    }

    internal class Workloads(ICollection<Workload> workloads) : IEnumerable<Workload>
    {
        private readonly Dictionary<string, Workload> _workloads = workloads.ToDictionary(w => w.Name, w => w);

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
