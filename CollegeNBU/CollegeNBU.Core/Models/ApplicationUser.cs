using Microsoft.AspNet.Identity.EntityFramework;

namespace CollegeNBU.Core.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;
}