namespace ContosoUniversity.Models
{
    using System;
    using System.Collections.Generic;

    public class Instructor : Person
    {
        public DateTime? HireDate { get; set; }

        public virtual ICollection<Course> Courses { get; set; }

        public virtual OfficeAssignment OfficeAssignment { get; set; }
    }
}