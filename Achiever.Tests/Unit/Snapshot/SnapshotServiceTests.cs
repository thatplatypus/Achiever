using Xunit;
using Achiever.Client.Services.Snapshot;
using System.Collections.Generic;
using Achiever.Shared.Goals.ViewModels;

namespace Achiever.Tests.Unit.Snapshot
{
    public class SnapshotServiceTests
    {

        [Fact]
        public void Can_Snapshot_Strings()
        {
            // Arrange
            var service = new SnapshotService();
            var partition = "testPartition";
            string value = "testValue";

            // Act
            var id = service.Snapshot(partition, value);

            // Assert
            var snapshot = service.GetSnapshot<string>(id, partition);
            Assert.Equal(value, snapshot);
        }

        [Fact]
        public void Can_Snapshot_Goals()
        {
            // Arrange
            var service = new SnapshotService();
            Guid goalId = Guid.NewGuid();
            Goal value = new()
            {
                Id = goalId,
                Title = "Test Goal",
                SubTasks =
                [
                    new SubTask
                    {
                        Id = Guid.NewGuid(),
                        Title = "Test SubTask",
                        GoalId = goalId,
                        Status = "New",
                        LastModified = DateTime.Now
                    }
                ]
            };

            // Act
            var id = service.Snapshot(value);

            // Assert
            var snapshot = service.GetSnapshot<Goal>(id);
            Assert.Equal(value.Title, snapshot?.Title);
            Assert.Equal(value.Id, snapshot?.Id);
            Assert.Equal(value.SubTasks[0].Title, snapshot?.SubTasks?[0].Title);
            Assert.Equal(value.SubTasks[0].Id, snapshot?.SubTasks?[0].Id);
            Assert.Equal(value.SubTasks[0].GoalId, snapshot?.SubTasks?[0].GoalId);
        }

        [Fact]
        public void Can_Delete_Snapshots()
        {
            // Arrange
            var service = new SnapshotService();
            var partition = "testPartition";
            service.Snapshot(partition, "testValue");

            // Act
            service.DeleteSnapshots(partition);

            // Assert
            var snapshots = service.GetSnapshots(partition);
            Assert.Empty(snapshots);
        }
    }
}