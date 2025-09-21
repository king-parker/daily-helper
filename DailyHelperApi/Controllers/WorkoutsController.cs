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
        public async Task<IEnumerable<WorkoutEntry>> GetAllAsync() => await _repository.GetAllAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<WorkoutEntry>> GetById(int id)
        {
            var entry = await _repository.GetByIdAsync(id);
            if (entry == null)
            {
                return NotFound();
            }

            return entry;
        }

        [HttpGet("exercise/{exercise}")]
        public async Task<IEnumerable<WorkoutEntry>> GetByExercise(string exercise) =>
            await _repository.GetByExerciseAsync(exercise);

        [HttpPost]
        public async Task<ActionResult<WorkoutEntry>> Add(WorkoutEntry entry)
        {
            var createdEntry = await _repository.AddAsync(entry);
            if (createdEntry == null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetById), new { id = createdEntry.Id }, createdEntry);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<WorkoutEntry>> Update(int id, WorkoutEntry updatedEntry)
        {
            var entry = await _repository.UpdateAsync(id, updatedEntry);
            if (entry == null)
            {
                return NotFound();
            }

            return entry;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _repository.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
