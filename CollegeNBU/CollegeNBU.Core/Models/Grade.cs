namespace CollegeNBU.Core.Models;

public class Grade
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public Student Student { get; set; } = null!;

    public int CourseId { get; set; }

    public Course Course { get; set; } = null!;

    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; } = null!;

    public decimal Value { get; set; }

    public DateTime DateGiven { get; set; }
}