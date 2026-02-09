namespace JwtApi.DTOs
{
    public class TaskItemDto
    {
        public DateTime Date { get; set; }
        public string TaskName { get; set; }
        public string Task { get; set; }
        public int Hours { get; set; }
        public string StatusValue { get; set; }
    }

}
