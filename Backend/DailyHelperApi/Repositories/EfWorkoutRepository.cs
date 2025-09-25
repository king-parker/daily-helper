using DailyHelperApi.Data;
using DailyHelperApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DailyHelperApi.Repositories
{
    public class EfWorkoutRepository : IWorkoutRepository
    {
        private readonly AppDbContext _context;

        public EfWorkoutRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WorkoutEntry>> GetAllAsync() =>
            await _context.Workouts.ToListAsync();

        public async Task<WorkoutEntry?> GetByIdAsync(int id) =>
            await _context.Workouts.FindAsync(id);

        public async Task<IEnumerable<WorkoutEntry>> GetByExerciseAsync(string exercise) =>
            await _context.Workouts
                .Where(w => w.Exercise.ToLower() == exercise.ToLower())
                .ToListAsync();

        public async Task<WorkoutEntry?> AddAsync(WorkoutEntry entry)
        {
            _context.Workouts.Add(entry);
            await _context.SaveChangesAsync();
            return entry;
        }

        public async Task<WorkoutEntry?> UpdateAsync(int id, WorkoutEntry newEntry)
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

            await _context.SaveChangesAsync();
            return existingEntry;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entry = _context.Workouts.Find(id);
            if (entry == null)
            {
                return false;
            }

            _context.Workouts.Remove(entry);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
