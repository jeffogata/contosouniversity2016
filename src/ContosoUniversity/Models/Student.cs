namespace ContosoUniversity.Models
{
    using System;
    using System.Collections.Generic;

    public class Student : Person
    {
        public DateTime? EnrollmentDate { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}