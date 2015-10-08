namespace ContosoUniversity.Domain
{
    using System;

    public class Instructor : Person
    {
        public DateTime? HireDate { get; set; }

        public string Office { get; set; }
    }
}