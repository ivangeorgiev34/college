using CollegeNBU.Core.Models;
using CollegeNBU.Data.Data;
using CollegeNBU.Web.ViewModels.Department;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeNBU.Web.Controllers;

[Authorize(Roles = "Admin")]
public class DepartmentController : Controller
{
    private readonly CollegeDbContext _context;

    public DepartmentController(CollegeDbContext context)
    {
        _context = context;
    }

    // LIST
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var departments = await _context.Departments
            .Include(d => d.Faculty)
            .Include(d => d.HeadTeacher)
            .Select(d => new DepartmentViewModel
            {
                Id = d.Id,
                Name = d.Name,
                FacultyName = d.Faculty.Name,
                HeadTeacherName = d.HeadTeacher != null
                    ? d.HeadTeacher.FirstName + " " + d.HeadTeacher.LastName
                    : null
            })
            .ToListAsync();

        return View(departments);
    }

    // CREATE GET
    public async Task<IActionResult> Create()
    {
        ViewBag.Faculties = await _context.Faculties.ToListAsync();
        ViewBag.Teachers = await _context.Teachers.ToListAsync();

        return View();
    }

    // CREATE POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateDepartmentViewModel model)
    {
        ViewBag.Faculties = await _context.Faculties.ToListAsync();
        ViewBag.Teachers = await _context.Teachers.ToListAsync();

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (_context.Departments.Any(x => x.HeadTeacherId == model.HeadTeacherId))
        {
            ModelState.AddModelError("", "A head teacher can be head at only one department");
            return View(model);
        }

        var department = new Department
        {
            Name = model.Name,
            FacultyId = model.FacultyId,
            HeadTeacherId = model.HeadTeacherId
        };

        _context.Departments.Add(department);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // EDIT GET
    public async Task<IActionResult> Edit(int id)
    {
        var department = await _context.Departments.FindAsync(id);

        if (department == null)
            return NotFound();

        var model = new EditDepartmentViewModel
        {
            Id = department.Id,
            Name = department.Name,
            FacultyId = department.FacultyId,
            HeadTeacherId = department.HeadTeacherId
        };

        ViewBag.Faculties = await _context.Faculties.ToListAsync();
        ViewBag.Teachers = await _context.Teachers.ToListAsync();

        return View(model);
    }

    // EDIT POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditDepartmentViewModel model)
    {
        var department = await _context.Departments.FindAsync(model.Id);

        if (department == null)
            return NotFound();

        department.Name = model.Name;
        department.FacultyId = model.FacultyId;
        department.HeadTeacherId = model.HeadTeacherId;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // DELETE
    public async Task<IActionResult> Delete(int id)
    {
        var department = await _context.Departments.FindAsync(id);

        if (department == null)
            return NotFound();

        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}