using CollegeNBU.Core.Models;
using CollegeNBU.Data.Data;
using CollegeNBU.Web.ViewModels.Teacher;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeNBU.Web.Controllers;

public class TeacherController : Controller
{
    private readonly CollegeDbContext _context;

    public TeacherController(CollegeDbContext context)
    {
        _context = context;
    }

    // LIST
    public async Task<IActionResult> Index()
    {
        var teachers = await _context.Teachers
            .Include(t => t.Department)
            .Select(t => new TeacherViewModel
            {
                Id = t.Id,
                FullName = t.FirstName + " " + t.LastName,
                Email = t.Email,
                DepartmentName = t.Department.Name,
                QualificationSubjects = t.QualificationSubjects
            })
            .ToListAsync();

        return View(teachers);
    }

    // CREATE GET
    public async Task<IActionResult> Create()
    {
        ViewBag.Departments = await _context.Departments.ToListAsync();
        return View();
    }

    // CREATE POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateTeacherViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Departments = await _context.Departments.ToListAsync();
            return View(model);
        }

        var teacher = new Teacher
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            QualificationSubjects = model.QualificationSubjects,
            DepartmentId = model.DepartmentId
        };

        _context.Teachers.Add(teacher);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // EDIT GET
    public async Task<IActionResult> Edit(int id)
    {
        var teacher = await _context.Teachers.FindAsync(id);

        if (teacher == null)
            return NotFound();

        var model = new EditTeacherViewModel
        {
            Id = teacher.Id,
            FirstName = teacher.FirstName,
            LastName = teacher.LastName,
            Email = teacher.Email,
            QualificationSubjects = teacher.QualificationSubjects,
            DepartmentId = teacher.DepartmentId
        };

        ViewBag.Departments = await _context.Departments.ToListAsync();

        return View(model);
    }

    // EDIT POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditTeacherViewModel model)
    {
        var teacher = await _context.Teachers.FindAsync(model.Id);

        if (teacher == null)
            return NotFound();

        teacher.FirstName = model.FirstName;
        teacher.LastName = model.LastName;
        teacher.Email = model.Email;
        teacher.QualificationSubjects = model.QualificationSubjects;
        teacher.DepartmentId = model.DepartmentId;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // DELETE
    public async Task<IActionResult> Delete(int id)
    {
        var teacher = await _context.Teachers.FindAsync(id);

        if (teacher == null)
            return NotFound();

        _context.Teachers.Remove(teacher);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}