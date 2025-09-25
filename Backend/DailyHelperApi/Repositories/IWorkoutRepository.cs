using DailyHelperApi.Models;

namespace DailyHelperApi.Repositories
{
    public interface IWorkoutRepository
    {
        Task<IEnumerable<WorkoutEntry>> GetAllAsync();
        Task<WorkoutEntry?> GetByIdAsync(int id);
        Task<IEnumerable<WorkoutEntry>> GetByExerciseAsync(string exercise);
        Task<WorkoutEntry?> AddAsync(WorkoutEntry entry);
        Task<WorkoutEntry?> UpdateAsync(int id, WorkoutEntry newEntry);
        Task<bool> DeleteAsync(int id);
    }
}
