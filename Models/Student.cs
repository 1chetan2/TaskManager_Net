namespace JwtApi.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string StudentName { get; set; }
        public string Email { get; set; }
        public string Course { get; set; }

        public DateTime EnrollmentDate { get; set; }
    }
}
