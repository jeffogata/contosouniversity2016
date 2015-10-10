namespace ContosoUniversity.Models
{
    public class Enrollment : Entity
    {
        public Student Student { get; set; }

        public int? Grade { get; set; }
    }
}