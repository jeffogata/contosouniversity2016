namespace ContosoUniversity.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Course : Entity
    {
        [Required, StringLength(5)]
        public string Number { get; set; }

        [Required, StringLength(50)]
        public string Title { get; set; }

        [Required]
        public int Credits { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }

        public virtual ICollection<CourseInstructor> CourseInstructors { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}