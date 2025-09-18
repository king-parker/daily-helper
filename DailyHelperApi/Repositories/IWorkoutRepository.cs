using DailyHelperApi.Models;

namespace DailyHelperApi.Repositories
{
    public interface IWorkoutRepository
    {
        IEnumerable<WorkoutEntry> GetAll();
        WorkoutEntry? GetById(int id);
        IEnumerable<WorkoutEntry> GetByExercise(string exercise);
        WorkoutEntry? Add(WorkoutEntry entry);
        WorkoutEntry? Update(int id, WorkoutEntry newEntry);
        bool Delete(int id);
    }
}
