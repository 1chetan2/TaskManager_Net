using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JwtApi.Models
{
    public class TaskChild
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("TaskItem")]
        public int TaskItemId { get; set; }

        [Required]
        public string StatusValue { get; set; }

        public TaskItem TaskItem { get; set; }

   
    }
}
