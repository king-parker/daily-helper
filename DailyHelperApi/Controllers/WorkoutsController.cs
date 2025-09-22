using AutoMapper;
using DailyHelperApi.Dtos.Workouts;
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
        private readonly IMapper _mapper;

        public WorkoutsController(IWorkoutRepository repository, ILogger<WorkoutsController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<WorkoutResponseDto>> GetAll()
        {
            var entries = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<WorkoutResponseDto>>(entries);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WorkoutResponseDto>> GetById(int id)
        {
            var entry = await _repository.GetByIdAsync(id);
            if (entry == null) { return NotFound(); }

            var response = _mapper.Map<WorkoutResponseDto>(entry);

            return Ok(response);
        }

        [HttpGet("exercise/{exercise}")]
        public async Task<IEnumerable<WorkoutResponseDto>> GetByExercise(string exercise)
        {
            var entries = await _repository.GetByExerciseAsync(exercise);
            return _mapper.Map<IEnumerable<WorkoutResponseDto>>(entries);
        }

        [HttpPost]
        public async Task<ActionResult<WorkoutResponseDto>> Add(WorkoutCreateDto workout)
        {
            var entry = _mapper.Map<WorkoutEntry>(workout);
            var createdEntry = await _repository.AddAsync(entry);
            if (createdEntry == null) {  return BadRequest(); }

            var workoutResponse = _mapper.Map<WorkoutResponseDto>(createdEntry);

            return CreatedAtAction(nameof(GetById), new { id = createdEntry.Id }, workoutResponse);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<WorkoutResponseDto>> Update(int id, WorkoutUpdateDto updatedWorkout)
        {
            // Check if the entry exists
            var entry = await _repository.GetByIdAsync(id);
            if (entry == null) { return NotFound(); }

            // Map updated fields
            _mapper.Map(updatedWorkout, entry);
            var updatedEntry = await _repository.UpdateAsync(id, entry);

            // Return the updated entry
            var response = _mapper.Map<WorkoutResponseDto>(updatedEntry);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _repository.DeleteAsync(id);
            if (!success) { return NotFound(); }

            return NoContent();
        }
    }
}
