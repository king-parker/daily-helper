using AutoMapper;
using DailyHelperApi.Dtos.Workouts;
using DailyHelperApi.Models;

namespace DailyHelperApi.Mappings
{
    public class WorkoutMappingProfile: Profile
    {
        public WorkoutMappingProfile()
        {
            CreateMap<WorkoutEntry, WorkoutResponseDto>().ReverseMap();
            CreateMap<WorkoutCreateDto, WorkoutEntry>().ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<WorkoutUpdateDto, WorkoutEntry>().ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
