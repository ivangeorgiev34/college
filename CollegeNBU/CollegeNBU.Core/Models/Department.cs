namespace CollegeNBU.Core.Models;

public class Department
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int FacultyId { get; set; }

    public Faculty Faculty { get; set; } = null!;

    public int? HeadTeacherId { get; set; }

    public Teacher? HeadTeacher { get; set; }

    public ICollection<Teacher> Teachers { get; set; }
        = new List<Teacher>();

    public ICollection<Course> Courses { get; set; }
        = new List<Course>();
}