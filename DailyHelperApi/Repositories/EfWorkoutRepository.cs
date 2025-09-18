using DailyHelperApi.Data;
using DailyHelperApi.Models;

namespace DailyHelperApi.Repositories
{
    public class EfWorkoutRepository : IWorkoutRepository
    {
        private readonly AppDbContext _context;

        public EfWorkoutRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<WorkoutEntry> GetAll() => _context.Workouts.ToList();

        public WorkoutEntry? GetById(int id) => _context.Workouts.Find(id);

        public IEnumerable<WorkoutEntry> GetByExercise(string exercise) =>
            _context.Workouts
                .Where(w => w.Exercise.ToLower() == exercise.ToLower())
                .ToList();

        public WorkoutEntry? Add(WorkoutEntry entry)
        {
            _context.Workouts.Add(entry);
            _context.SaveChanges();
            return entry;
        }

        public WorkoutEntry? Update(int id, WorkoutEntry newEntry)
        {
            var existingEntry = _context.Workouts.Find(id);
            if (existingEntry == null)
            {
                return null;
            }

            existingEntry.Exercise = newEntry.Exercise;
            existingEntry.Weight = newEntry.Weight;
            existingEntry.Sets = newEntry.Sets;
            existingEntry.Reps = newEntry.Reps;
            existingEntry.Date = newEntry.Date;
            existingEntry.Notes = newEntry.Notes;

            _context.SaveChanges();
            return existingEntry;
        }

        public bool Delete(int id)
        {
            var entry = _context.Workouts.Find(id);
            if (entry == null)
            {
                return false;
            }

            _context.Workouts.Remove(entry);
            _context.SaveChanges();
            return true;
        }
    }
}
