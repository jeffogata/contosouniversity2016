namespace ContosoUniversity.Models
{
    using System.Collections.Generic;

    public class Course : Entity
    {
        public string Number { get; set; }

        public string Title { get; set; }

        public int Credits { get; set; }

        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }

        public virtual ICollection<CourseInstructor> CourseInstructors { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}