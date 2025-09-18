using DailyHelperApi.Models;
using DailyHelperApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DailyHelperApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutsController : ControllerBase
    {
        private readonly IWorkoutRepository _repository;
        private readonly ILogger<WorkoutsController> _logger;

        public WorkoutsController(IWorkoutRepository repository, ILogger<WorkoutsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WorkoutEntry> GetAll() => _repository.GetAll();

        [HttpGet("{id}")]
        public ActionResult<WorkoutEntry> GetById(int id)
        {
            var entry = _repository.GetById(id);
            if (entry == null)
            {
                return NotFound();
            }

            return entry;
        }

        [HttpGet("exercise/{exercise}")]
        public IEnumerable<WorkoutEntry> GetByExercise(string exercise) =>
            _repository.GetByExercise(exercise);

        [HttpPost]
        public ActionResult<WorkoutEntry> Add(WorkoutEntry entry)
        {
            var createdEntry = _repository.Add(entry);
            if (createdEntry == null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetById), new { id = createdEntry.Id }, createdEntry);
        }

        [HttpPut("{id}")]
        public ActionResult<WorkoutEntry> Update(int id, WorkoutEntry updatedEntry)
        {
            var entry = _repository.Update(id, updatedEntry);
            if (entry == null)
            {
                return NotFound();
            }

            return entry;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var success = _repository.Delete(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
