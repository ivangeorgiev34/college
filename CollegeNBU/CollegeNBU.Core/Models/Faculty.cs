namespace CollegeNBU.Core.Models;

public class Faculty
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int CollegeId { get; set; }

    public College College { get; set; } = null!;

    public ICollection<Department> Departments { get; set; }
        = new List<Department>();
}