using AutoMapper;
using DailyHelperApi.Dtos.Workouts;
using DailyHelperApi.Models;

namespace DailyHelperApi.Mappings
{
    public class WorkoutMappingProfile: Profile
    {
        public WorkoutMappingProfile()
        {
            CreateMap<WorkoutCreateDto, WorkoutEntry>();
            CreateMap<WorkoutUpdateDto, WorkoutEntry>();
            CreateMap<WorkoutEntry, WorkoutResponseDto>();
        }
    }
}
