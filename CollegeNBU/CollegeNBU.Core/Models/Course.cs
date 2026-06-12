using System.Diagnostics;

namespace CollegeNBU.Core.Models;

public class Course
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Credits { get; set; }

    public int DepartmentId { get; set; }

    public Department Department { get; set; } = null!;

    public int TeacherId { get; set; }

    public Teacher Teacher { get; set; } = null!;

    public ICollection<StudentCourse> StudentCourses { get; set; }
        = new List<StudentCourse>();

    public ICollection<Grade> Grades { get; set; }
        = new List<Grade>();

    public ICollection<Attendance> Attendances { get; set; }
        = new List<Attendance>();

    public ICollection<SemesterProgramCourse> SemesterPrograms { get; set; }
        = new List<SemesterProgramCourse>();
}