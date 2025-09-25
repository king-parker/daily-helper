using AutoMapper;
using DailyHelperApi.Dtos.Workouts;
using DailyHelperApi.Mappings;
using DailyHelperApi.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace DailyHelperApi.Tests.Dtos
{
    public class WorkoutMappingTests
    {
        private readonly IMapper _mapper;

        public WorkoutMappingTests()
        {
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(WorkoutMappingProfile).Assembly);
            }, loggerFactory);

            config.AssertConfigurationIsValid(); // Ensure the configuration is valid
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void WorkoutEntry_To_WorkoutResponseDto_MapsCorrectly()
        {
            // Arrange
            var workout = new WorkoutEntry
            {
                Id = 5,
                Exercise = "Goblet Squat",
                Weight = 30,
                Sets = 3,
                Reps = 12,
                Date = new DateTime(2025, 9, 18),
                Notes = "Test notes"
            };

            // Act
            var dto = _mapper.Map<WorkoutResponseDto>(workout);

            // Assert
            dto.Should().NotBeNull();
            dto.Id.Should().Be(workout.Id);
            dto.Exercise.Should().Be(workout.Exercise);
            dto.Weight.Should().Be(workout.Weight);
            dto.Sets.Should().Be(workout.Sets);
            dto.Reps.Should().Be(workout.Reps);
            dto.Date.Should().Be(workout.Date);
            dto.Notes.Should().Be(workout.Notes);
        }

        [Fact]
        public void WorkoutResponseDto_To_WorkoutEntry_MapsCorrectly()
        {
            // Arrange
            var dto = new WorkoutResponseDto
            {
                Id = 10,
                Exercise = "Romanian Deadlift",
                Weight = 45,
                Sets = 3,
                Reps = 10,
                Date = new DateTime(2025, 9, 18),
                Notes = "Test notes again"
            };

            // Act
            var workout = _mapper.Map<WorkoutEntry>(dto);

            // Assert
            workout.Should().NotBeNull();
            workout.Id.Should().Be(dto.Id);
            workout.Exercise.Should().Be(dto.Exercise);
            workout.Weight.Should().Be(dto.Weight);
            workout.Sets.Should().Be(dto.Sets);
            workout.Reps.Should().Be(dto.Reps);
            workout.Date.Should().Be(dto.Date);
            workout.Notes.Should().Be(dto.Notes);
        }

        [Fact]
        public void WorkoutCreateDto_To_WorkoutEntry_MapsCorrectly()
        {
            // Arrange
            var createDto = new WorkoutCreateDto
            {
                Exercise = "Bulgarian Split Squat",
                Weight = 10,
                Sets = 3,
                Reps = 8,
                Date = new DateTime(2025, 9, 18),
                Notes = "Creating a new workout"
            };

            // Act
            var workout = _mapper.Map<WorkoutEntry>(createDto);

            // Assert
            workout.Should().NotBeNull();
            workout.Id.Should().Be(0); // Id should be default as it's not set in create DTO
            workout.Exercise.Should().Be(createDto.Exercise);
            workout.Weight.Should().Be(createDto.Weight);
            workout.Sets.Should().Be(createDto.Sets);
            workout.Reps.Should().Be(createDto.Reps);
            workout.Date.Should().Be(createDto.Date);
            workout.Notes.Should().Be(createDto.Notes);
        }

        [Fact]
        public void WorkoutUpdateDto_To_WorkoutEntry_MapsCorrectly()
        {
            // Arrange
            var updateDto = new WorkoutUpdateDto
            {
                Exercise = "Calf Raises",
                Weight = 90,
                Sets = 3,
                Reps = 15,
                Date = new DateTime(2025, 9, 18),
                Notes = "Updating an existing workout"
            };

            // Act
            var workout = _mapper.Map<WorkoutEntry>(updateDto);

            // Assert
            workout.Should().NotBeNull();
            workout.Id.Should().Be(0); // Id should be default as it's not set in update DTO
            workout.Exercise.Should().Be(updateDto.Exercise);
            workout.Weight.Should().Be(updateDto.Weight);
            workout.Sets.Should().Be(updateDto.Sets);
            workout.Reps.Should().Be(updateDto.Reps);
            workout.Date.Should().Be(updateDto.Date);
            workout.Notes.Should().Be(updateDto.Notes);
        }
    }
}