namespace ContosoUniversity.Domain
{
    using System.Collections.Generic;

    public class Course : Entity
    {
        public string Title { get; set; }

        public int Credits { get; set; }

        public Department Department { get; set; }

        public ICollection<Instructor> Instructors { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}