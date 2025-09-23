using DailyHelperApi.Data;
using DailyHelperApi.Models;
using DailyHelperApi.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DailyHelperApi.Tests.Repositories
{
    public class EfWorkoutRepositoryTests
    {
        [Fact]
        public async Task GetAllAsync_MultipleEntries_ReturnsList()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new EfWorkoutRepository(context);
            var workouts = SeedSmallDataSet(context);

            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should(). HaveCount(workouts.Length);
            result.Should().BeEquivalentTo(
                workouts,
                options => options.Excluding(w => w.Id)
             );
        }

        [Fact]
        public async Task GetAllAsync_SingleEntry_ReturnsListWithOneItem()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new EfWorkoutRepository(context);
            var workout = new WorkoutEntry
            {
                Exercise = "Goblet Squat",
                Weight = 30,
                Sets = 3,
                Reps = 12
            };
            context.Workouts.Add(workout);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);
            result.First().Should().BeEquivalentTo(
                workout,
                options => options.Excluding(w => w.Id)
             );
        }

        [Fact]
        public async Task GetAllAsync_NoEntries_ReturnsEmptyList()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new EfWorkoutRepository(context);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        public async Task GetByIdAsync_ExistingId_ReturnsEntry(int id)
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new EfWorkoutRepository(context);
            var workouts = SeedSmallDataSet(context);

            var assignedIds = context.Workouts.Select(w => w.Id).ToArray();
            var requestedId = assignedIds[id - 1];

            // Act
            var result = await repository.GetByIdAsync(requestedId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(
                workouts[id - 1],
                options => options.Excluding(w => w.Id)
             );
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new EfWorkoutRepository(context);
            SeedSmallDataSet(context);
            var nonExistingId = 999;

            // Act
            var result = await repository.GetByIdAsync(nonExistingId);

            // Assert
            result.Should().BeNull();
        }

        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        private WorkoutEntry[] SeedSmallDataSet(AppDbContext context)
        {
            var workouts = new[] {
                new WorkoutEntry
                {
                    Exercise = "Goblet Squat",
                    Weight = 30,
                    Sets = 3,
                    Reps = 12
                },
                new WorkoutEntry
                {
                    Exercise = "Romanian Deadlift",
                    Weight = 45,
                    Sets = 3,
                    Reps = 10
                },
                new WorkoutEntry
                {
                    Exercise = "Bulgarian Split Squat",
                    Weight = 10,
                    Sets = 3,
                    Reps = 8,
                    Notes = "Could use heavier weight"
                },
                new WorkoutEntry
                {
                    Exercise = "Calf Raises",
                    Weight = 90,
                    Sets = 3,
                    Reps = 15
                },
                new WorkoutEntry
                {
                    Exercise = "Hip Thrusts",
                    Weight = 30,
                    Sets = 3,
                    Reps = 10
                }
            };

            context.Workouts.AddRange(workouts);
            context.SaveChanges();

            return workouts;
        }
    }
}