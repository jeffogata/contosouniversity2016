namespace ContosoUniversity.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class CourseInstructor
    {
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }
        [ForeignKey("Instructor")]
        public int InstructorId { get; set; }
        public Instructor Instructor { get; set; }
    }
}