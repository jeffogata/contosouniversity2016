namespace ContosoUniversity.Models
{
    using System;

    public class Department : Entity
    {
        public string Name { get; set; }

        public decimal Budget { get; set; }

        public DateTime StartDate { get; set; }

        //public Instructor Administrator { get; set; }
    }
}