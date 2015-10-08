namespace ContosoUniversity.Domain
{
    public class Enrollment : Entity
    {
        public Student Student { get; set; }

        public int? Grade { get; set; }
    }
}