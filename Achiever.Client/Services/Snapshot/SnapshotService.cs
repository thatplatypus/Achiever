using System.Text.Json;

namespace Achiever.Client.Services.Snapshot
{
    public class SnapshotService : ISnapshotService
    {
        private readonly Dictionary<string, List<Snapshot>> _cache = [];

        public List<Snapshot> GetSnapshots(string partition)
        {
            if(_cache.TryGetValue(partition, out List<Snapshot>? snapshots))
            {
                return snapshots;
            }
            else
            {
                return [];
            }
        }

        public Guid Snapshot<T>(string partition, T value)
        {
            Guid id = Guid.NewGuid();
            string data = JsonSerializer.Serialize(value);

            if (_cache.TryGetValue(partition, out List<Snapshot>? snapshots))
            {
                snapshots?.Add(new Snapshot(id, data));
            }
            else
            {
                _cache[partition] = [new Snapshot(id, data)];
            }

            return id;
        }

        public void DeleteSnapshots(string partition)
        {
            _cache.Remove(partition);
        }
    }
}
