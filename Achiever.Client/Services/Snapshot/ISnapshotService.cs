using System.Text.Json;

namespace Achiever.Client.Services.Snapshot
{
    public interface ISnapshotService
    {
        public Guid Snapshot<T>(string partition, T value);

        public List<Snapshot> GetSnapshots(string partition);

        public void DeleteSnapshots(string partition);
    }

    public static class ISnapshotServiceExtensions
    {
        public static Guid Snapshot<T>(this ISnapshotService snapshotService, T value)
        {
            return snapshotService.Snapshot(typeof(T).Name, value);
        }

        public static T? GetSnapshot<T>(this ISnapshotService snapshotService, Guid id)
        {
            return JsonSerializer.Deserialize<T>(snapshotService.GetSnapshots(typeof(T).Name).FirstOrDefault(x => x.Id == id)?.Data);
        }

        public static T? GetSnapshot<T>(this ISnapshotService snapshotService, Guid id, string partition)
        {
            return JsonSerializer.Deserialize<T>(snapshotService.GetSnapshots(partition).FirstOrDefault(x => x.Id == id)?.Data);
        }

        public static T? GetLatest<T>(this ISnapshotService snapshotService)
        {
            return JsonSerializer.Deserialize<T>(snapshotService.GetSnapshots(typeof(T).Name).Last().Data);
        }

        public static void DeleteSnapshots<T>(this ISnapshotService snapshotService)
        {
            snapshotService.DeleteSnapshots(typeof(T).Name);
        }
    }   
}
