// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CollegeNBU.Core.Models;
using CollegeNBU.Data.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CollegeNBU.Areas.Identity.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly IUserEmailStore<ApplicationUser> _emailStore;
    private readonly ILogger<RegisterModel> _logger;
    private readonly IEmailSender _emailSender;
    private readonly CollegeDbContext _context;
    public RegisterModel(
        UserManager<ApplicationUser> userManager,
        IUserStore<ApplicationUser> userStore,
        SignInManager<ApplicationUser> signInManager,
        ILogger<RegisterModel> logger,
        IEmailSender emailSender,
        CollegeDbContext context)
    {
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = GetEmailStore();
        _signInManager = signInManager;
        _logger = logger;
        _emailSender = emailSender;
        _context = context;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; } = default!;

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string? ReturnUrl { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public IList<AuthenticationScheme>? ExternalLogins { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class InputModel
    {
        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = default!;

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = default!;

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        public string? Specialty { get; set; }

        public string? QualificationSubjects { get; set; }

        [Required]
        public string? Role { get; set; }


        [Display(Name = "Department")]
        public List<DepartmentModel> Departments { get; set; } = new List<DepartmentModel>();

        public int SelectedDepartmentId { get; set; }
    }

    public class DepartmentModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
    }


    public async Task OnGetAsync(string? returnUrl = null)
    {
        Input = new InputModel();

        Input.Departments = _context.Departments
            .Select(x => new DepartmentModel
            {
                Id = x.Id,
                Name = x.Name,
            })
            .ToList();

        ReturnUrl = returnUrl;
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        if (ModelState.IsValid)
        {
            var user = CreateUser();

            await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                var userId = await _userManager.GetUserIdAsync(user);

                if (Input.Role != null)
                    await _userManager.AddToRoleAsync(user, Input.Role);

                if (Input.Role == "Student")
                {
                    Student student = new()
                    {
                        FirstName = Input.FirstName,
                        LastName = Input.LastName,
                        Email = Input.Email,
                        Specialty = Input.Specialty,
                        UserId = userId,
                        DepartmentId = Input.SelectedDepartmentId,
                    };

                    await _context.AddAsync(student);
                }
                else if (Input.Role == "Teacher")
                {
                    Teacher teacher = new()
                    {
                        FirstName = Input.FirstName,
                        LastName = Input.LastName,
                        Email = Input.Email,
                        QualificationSubjects = Input.QualificationSubjects,
                        UserId = userId,
                        DepartmentId = Input.SelectedDepartmentId,
                    };

                    await _context.AddAsync(teacher);
                }
                else if (Input.Role == "DepartmentHead")
                {
                    Teacher teacher = new()
                    {
                        FirstName = Input.FirstName,
                        LastName = Input.LastName,
                        Email = Input.Email,
                        UserId = userId,
                        DepartmentId = Input.SelectedDepartmentId,
                    };

                    await _context.AddAsync(teacher);
                }
                else if (Input.Role == "Rector")
                {
                    Rector teacher = new()
                    {
                        FirstName = Input.FirstName,
                        LastName = Input.LastName,
                        Email = Input.Email,
                        UserId = userId,
                    };

                    await _context.AddAsync(teacher);
                }

                await _context.SaveChangesAsync();

                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                }
                else
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }

    private ApplicationUser CreateUser()
    {
        try
        {
            return new()
            {
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                Email = Input.Email,
            };
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }

    private IUserEmailStore<ApplicationUser> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<ApplicationUser>)_userStore;
    }
}
