using System.Diagnostics;

namespace CollegeNBU.Core.Models;

public class Student
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Specialty { get; set; } = null!;

    public string? UserId { get; set; }

    public ApplicationUser? User { get; set; }

    public ICollection<StudentCourse> StudentCourses { get; set; }
        = new List<StudentCourse>();

    public ICollection<Grade> Grades { get; set; }
        = new List<Grade>();

    public ICollection<Attendance> Attendances { get; set; }
        = new List<Attendance>();
}