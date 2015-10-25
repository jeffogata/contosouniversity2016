namespace ContosoUniversity.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;

    public class Department : Entity
    {
        [Required, StringLength(50)]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        public int? InstructorId { get; set; }
        
        //[Timestamp]  uncomment to use optimistic concurrency
        // Set this to computed for now (see context configuration)
        public byte[] RowVersion { get; set; }

        public virtual Instructor Administrator { get; set; }

        public virtual ICollection<Course> Courses { get; set; } 
    }
}