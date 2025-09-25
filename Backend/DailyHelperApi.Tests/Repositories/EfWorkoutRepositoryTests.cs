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
            var workouts = await SeedSmallDataSetAsync(context);

            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(workouts.Count);
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
            var workouts = await SeedSmallDataSetAsync(context);

            var assignedIds = context.Workouts.Select(w => w.Id).ToList();
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
            await SeedSmallDataSetAsync(context);
            var nonExistingId = 999;

            // Act
            var result = await repository.GetByIdAsync(nonExistingId);

            // Assert
            result.Should().BeNull();
        }

        [Theory]
        [InlineData("Goblet Squat")]
        [InlineData("Romanian Deadlift")]
        [InlineData("Calf Raises")]
        [InlineData("Bulgarian Split Squat")]
        public async Task GetByExerciseAsync_ExistingExercise_ReturnsAllMatchingEntries(string exercise)
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new EfWorkoutRepository(context);
            var workouts = await SeedSmallDataSetAsync(context);

            var duplicateEntries = new List<WorkoutEntry> {
                new() 
                {
                    Exercise = "Calf Raises",
                    Weight = 35,
                    Sets = 4,
                    Reps = 10
                },
                new() 
                {
                    Exercise = "Calf Raises",
                    Weight = 40,
                    Sets = 3,
                    Reps = 8
                },
                new() 
                {
                    Exercise = "Romanian Deadlift",
                    Weight = 32.5,
                    Sets = 3,
                    Reps = 10
                }
            };
            context.Workouts.AddRange(duplicateEntries);
            await context.SaveChangesAsync();
            workouts.AddRange(duplicateEntries);

            // Act
            var result = await repository.GetByExerciseAsync(exercise);

            // Assert
            var expectedWorkouts = workouts.Where(w => w.Exercise.Equals(exercise, StringComparison.OrdinalIgnoreCase)).ToList();
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(expectedWorkouts.Count);
            result.Should().BeEquivalentTo(
                expectedWorkouts,
                options => options.Excluding(w => w.Id)
             );
        }

        [Fact]
        public async Task GetByExerciseAsync_NonExistingExercise_ReturnsEmptyList()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new EfWorkoutRepository(context);
            await SeedSmallDataSetAsync(context);
            var nonExistingExercise = "NonExistingExercise";

            // Act
            var result = await repository.GetByExerciseAsync(nonExistingExercise);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Theory]
        [InlineData("goblet squat")]
        [InlineData("GOBLET SQUAT")]
        [InlineData("GoBlEt SqUaT")]
        public async Task GetByExerciseAsync_ExerciseCaseInsensitive_ReturnsMatchingEntries(string exercise)
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new EfWorkoutRepository(context);
            var workouts = await SeedSmallDataSetAsync(context);

            // Act
            var result = await repository.GetByExerciseAsync(exercise);

            // Assert
            var expectedWorkouts = workouts.Where(w => w.Exercise.Equals("Goblet Squat", StringComparison.OrdinalIgnoreCase)).ToList();
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(expectedWorkouts.Count);
            result.Should().BeEquivalentTo(
                expectedWorkouts,
                options => options.Excluding(w => w.Id)
             );
        }

        [Fact]
        public async Task AddAsync_ValidEntry_AddsToEmptyDatabase()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new EfWorkoutRepository(context);
            var newWorkout = new WorkoutEntry
            {
                Exercise = "Goblet Squat",
                Weight = 30,
                Sets = 3,
                Reps = 12
            };

            var initialCount = context.Workouts.Count();

            // Act
            var result = await repository.AddAsync(newWorkout);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(
                newWorkout,
                options => options.Excluding(w => w.Id)
             );
            result.Id.Should().BeGreaterThan(0);
            result.Id.Should().Be(initialCount + 1);

            context.Workouts.Count().Should().Be(initialCount + 1);

            var dbEntry = context.Workouts.Find(result.Id);
            dbEntry.Should().NotBeNull();
            dbEntry.Should().BeEquivalentTo(
                newWorkout,
                options => options.Excluding(w => w.Id)
             );
        }

        [Fact]
        public async Task AddAsync_ValidEntry_AddsToNonEmptyDatabase()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new EfWorkoutRepository(context);
            await SeedSmallDataSetAsync(context);
            var newWorkout = new WorkoutEntry
            {
                Exercise = "Overhead Press",
                Weight = 20,
                Sets = 3,
                Reps = 8
            };

            var initialCount = context.Workouts.Count();

            // Act
            var result = await repository.AddAsync(newWorkout);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(
                newWorkout,
                options => options.Excluding(w => w.Id)
             );
            result.Id.Should().BeGreaterThan(0);
            result.Id.Should().Be(initialCount + 1);

            context.Workouts.Count().Should().Be(initialCount + 1);

            var dbEntry = context.Workouts.Find(result.Id);
            dbEntry.Should().NotBeNull();
            dbEntry.Should().BeEquivalentTo(
                newWorkout,
                options => options.Excluding(w => w.Id)
             );
        }

        [Fact]
        public async Task UpdateAsync_ValidIdAndEntry_UpdatesEntry()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new EfWorkoutRepository(context);
            var workouts = await SeedSmallDataSetAsync(context);
            var assignedIds = context.Workouts.Select(w => w.Id).ToList();
            var targetIndex = 2; // Update the 3rd entry
            var targetId = assignedIds[targetIndex];
            var entryCount = context.Workouts.Count();
            var updatedWorkout = new WorkoutEntry
            {
                Exercise = "Updated Exercise",
                Weight = 100,
                Sets = 5,
                Reps = 5,
                Notes = "Updated notes"
            };

            // Act
            var result = await repository.UpdateAsync(targetId, updatedWorkout);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(targetId);
            result.Should().BeEquivalentTo(
                updatedWorkout,
                options => options.Excluding(w => w.Id)
             );

            var dbEntry = context.Workouts.Find(targetId);
            dbEntry.Should().NotBeNull();
            dbEntry.Should().BeEquivalentTo(
                updatedWorkout,
                options => options.Excluding(w => w.Id)
             );

            context.Workouts.Count().Should().Be(entryCount);
        }

        [Fact]
        public async Task UpdateAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new EfWorkoutRepository(context);
            await SeedSmallDataSetAsync(context);
            var nonExistingId = 999;
            var entryCount = context.Workouts.Count();
            var updatedWorkout = new WorkoutEntry
            {
                Exercise = "Updated Exercise",
                Weight = 100,
                Sets = 5,
                Reps = 5,
                Notes = "Updated notes"
            };

            // Act
            var result = await repository.UpdateAsync(nonExistingId, updatedWorkout);

            // Assert
            result.Should().BeNull();
            context.Workouts.Count().Should().Be(entryCount);
        }

        [Fact]
        public async Task DeleteAsync_ExistingId_DeletesEntry()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new EfWorkoutRepository(context);
            await SeedSmallDataSetAsync(context);
            var assignedIds = context.Workouts.Select(w => w.Id).ToList();
            var targetIndex = 1; // Delete the 2nd entry
            var targetId = assignedIds[targetIndex];
            var entryCount = context.Workouts.Count();

            // Act
            var result = await repository.DeleteAsync(targetId);

            // Assert
            result.Should().BeTrue();
            context.Workouts.Count().Should().Be(entryCount - 1);
            var dbEntry = context.Workouts.Find(targetId);
            dbEntry.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_NonExistingId_ReturnsFalse()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new EfWorkoutRepository(context);
            await SeedSmallDataSetAsync(context);
            var nonExistingId = 999;
            var entryCount = context.Workouts.Count();

            // Act
            var result = await repository.DeleteAsync(nonExistingId);

            // Assert
            result.Should().BeFalse();
            context.Workouts.Count().Should().Be(entryCount);
        }

        [Fact]
        public async Task DeleteAsync_EmptyDatabase_ReturnsFalse()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new EfWorkoutRepository(context);
            var nonExistingId = 1;
            var entryCount = context.Workouts.Count();

            // Act
            var result = await repository.DeleteAsync(nonExistingId);

            // Assert
            result.Should().BeFalse();
            context.Workouts.Count().Should().Be(entryCount);
        }

        [Fact]
        public async Task DeleteAsync_DeleteTwice_ReturnsFalseSecondDelete()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new EfWorkoutRepository(context);
            await SeedSmallDataSetAsync(context);
            var assignedIds = context.Workouts.Select(w => w.Id).ToList();
            var targetIndex = 0; // Delete the 1st entry
            var targetId = assignedIds[targetIndex];
            var entryCount = context.Workouts.Count();

            // Act
            var firstDeleteResult = await repository.DeleteAsync(targetId);
            var secondDeleteResult = await repository.DeleteAsync(targetId);

            // Assert
            firstDeleteResult.Should().BeTrue();
            secondDeleteResult.Should().BeFalse();
            context.Workouts.Count().Should().Be(entryCount - 1);
            var dbEntry = context.Workouts.Find(targetId);
            dbEntry.Should().BeNull();
        }

        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        private async Task<List<WorkoutEntry>> SeedSmallDataSetAsync(AppDbContext context)
        {
            var workouts = new List<WorkoutEntry> {
                new()
                {
                    Exercise = "Goblet Squat",
                    Weight = 30,
                    Sets = 3,
                    Reps = 12
                },
                new()
                {
                    Exercise = "Romanian Deadlift",
                    Weight = 45,
                    Sets = 3,
                    Reps = 10
                },
                new()
                {
                    Exercise = "Bulgarian Split Squat",
                    Weight = 10,
                    Sets = 3,
                    Reps = 8,
                    Notes = "Could use heavier weight"
                },
                new()
                {
                    Exercise = "Calf Raises",
                    Weight = 90,
                    Sets = 3,
                    Reps = 15
                },
                new()
                {
                    Exercise = "Hip Thrusts",
                    Weight = 30,
                    Sets = 3,
                    Reps = 10
                }
            };

            context.Workouts.AddRange(workouts);
            await context.SaveChangesAsync();

            return workouts;
        }
    }
}