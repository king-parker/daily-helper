using DailyHelperApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DailyHelperApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutsController : ControllerBase
    {
        // In-memory list to simulate a database, delete when done testing
        private static IEnumerable<WorkoutEntry> _dummyData = new List<WorkoutEntry> // Dummy data
        {
            new()
            {
                Id = 1,
                Exercise = "Goblet Squat",
                Weight = 30.0,
                Sets = 3,
                Reps = 12,
                Date = DateTime.Now
            },
            new()
            {
                Id = 2,
                Exercise = "Romanian Deadlift",
                Weight = 45.0,
                Sets = 3,
                Reps = 10,
                Date = DateTime.Now
            },
            new()
            {
                Id = 3,
                Exercise = "Bulgarian Split Squat",
                Weight = 10.0,
                Sets = 3,
                Reps = 8,
                Date = DateTime.Now,
                Notes = "Could use heavier weight"
            },
            new()
            {
                Id = 4,
                Exercise = "Calf Raises",
                Weight = 90.0,
                Sets = 3,
                Reps = 15,
                Date = DateTime.Now
            }
        };

        private readonly ILogger<WorkoutsController> _logger;

        public WorkoutsController(ILogger<WorkoutsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WorkoutEntry> GetAll()
        {
            return _dummyData;
        }

        [HttpGet("{id}")]
        public ActionResult<WorkoutEntry> GetById(int id)
        {
            var entry = _dummyData.FirstOrDefault(e => e.Id == id);
            if (entry == null)
            {
                return NotFound();
            }
            return entry;
        }

        [HttpGet("exercise/{exercise}")]
        public IEnumerable<WorkoutEntry> GetByExercise(string exercise)
        {
            return _dummyData.Where(e => e.Exercise.Equals(exercise, StringComparison.OrdinalIgnoreCase));
        }

        [HttpPost]
        public WorkoutEntry Add(WorkoutEntry entry)
        {
            // In a real application, you would save the entry to a database
            var newId = _dummyData.Max(e => e.Id) + 1;
            entry.Id = newId;
            _dummyData = _dummyData.Append(entry);
            return entry;
        }

        [HttpPut("{id}")]
        public ActionResult<WorkoutEntry> Update(int id, WorkoutEntry updatedEntry)
        {
            var entry = _dummyData.FirstOrDefault(e => e.Id == id);
            if (entry == null)
            {
                return NotFound();
            }

            // In a real application, you would update the entry in the database
            entry.Exercise = updatedEntry.Exercise;
            entry.Weight = updatedEntry.Weight;
            entry.Sets = updatedEntry.Sets;
            entry.Reps = updatedEntry.Reps;
            entry.Date = updatedEntry.Date;
            entry.Notes = updatedEntry.Notes;
            return entry;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entry = _dummyData.FirstOrDefault(e => e.Id == id);
            if (entry == null)
            {
                return NotFound();
            }

            // In a real application, you would delete the entry from the database
            _dummyData = _dummyData.Where(e => e.Id != id);
            return NoContent();
        }
    }
}
