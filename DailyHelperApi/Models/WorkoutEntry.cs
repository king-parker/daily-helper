namespace DailyHelperApi.Models
{
    public class WorkoutEntry
    {
        public int Id { get; set; }
        public string Exercise { get; set; } = string.Empty;
        public double Weight { get; set; } // in kg
        public int Sets { get; set; }
        public int Reps { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}