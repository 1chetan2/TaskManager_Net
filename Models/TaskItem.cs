using System.ComponentModel.DataAnnotations;

namespace JwtApi.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string TaskName { get; set; }

        [Required]
        public string Task { get; set; }

        [Range(0, 24)]
        public int Hours { get; set; }

        public TaskChild TaskStatus { get; set; }
    }
}
