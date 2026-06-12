namespace CollegeNBU.Core.Models;

public class Teacher
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? QualificationSubjects { get; set; }

    public int DepartmentId { get; set; }

    public Department Department { get; set; } = null!;

    public string? UserId { get; set; }

    public ApplicationUser? User { get; set; }

    public ICollection<Course> Courses { get; set; }
        = new List<Course>();
}