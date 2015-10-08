namespace ContosoUniversity.Domain
{
    using System;

    public class Student : Person
    {
        public DateTime? EnrollmentDate { get; set; }
    }
}