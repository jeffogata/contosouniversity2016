namespace ContosoUniversity.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Department : Entity
    {
        [Required, StringLength(50)]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        public int? InstructorId { get; set; }
        
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual Instructor Administrator { get; set; }

        public virtual ICollection<Course> Courses { get; set; } 
    }
}