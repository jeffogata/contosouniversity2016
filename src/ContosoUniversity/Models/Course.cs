namespace ContosoUniversity.Models
{
    using System.Collections.Generic;

    public class Course : Entity
    {
        public string Title { get; set; }

        public int Credits { get; set; }

        public int DepartmentID { get; set; }

        public virtual Department Department { get; set; }

        public virtual ICollection<Instructor> Instructors { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}