using CollegeNBU.Core.Models;
using CollegeNBU.Data.Data;
using CollegeNBU.Web.ViewModels.Grade;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CollegeNBU.Web.Controllers;

public class GradeController : Controller
{
    private readonly CollegeDbContext _context;

    public GradeController(CollegeDbContext context)
    {
        _context = context;
    }
    [Authorize(Roles = "Admin, Teacher, Student")]
    public async Task<IActionResult> Index()
    {
        var grades = await _context.Grades
            .Include(g => g.Student)
            .Include(g => g.Course)
            .Include(g => g.Teacher)
            .Select(g => new GradeViewModel
            {
                Id = g.Id,
                StudentName = g.Student.FirstName + " " + g.Student.LastName,
                CourseName = g.Course.Name,
                Value = g.Value,
                TeacherName = g.Teacher.FirstName + " " + g.Teacher.LastName
            })
            .ToListAsync();

        return View(grades);
    }

    // CREATE GET
    [Authorize(Roles = "Admin, Teacher")]
    public async Task<IActionResult> Create()
    {
        ViewBag.Students = await _context.Students.ToListAsync();
        ViewBag.Courses = await _context.Courses.ToListAsync();

        return View();
    }

    // CREATE POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin, Teacher")]
    public async Task<IActionResult> Create(CreateGradeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Students = await _context.Students.ToListAsync();
            ViewBag.Courses = await _context.Courses.ToListAsync();
            return View(model);
        }

        var userEmail = User.FindFirstValue(ClaimTypes.Email);

        var teacher = await _context.Teachers
            .FirstOrDefaultAsync(t => t.Email == userEmail);

        if (teacher == null)
            return Unauthorized();

        var course = await _context.Courses
            .FirstOrDefaultAsync(c => c.Id == model.CourseId && c.TeacherId == teacher.Id);

        if (course == null)
            return Forbid();

        var grade = new Grade
        {
            StudentId = model.StudentId,
            CourseId = model.CourseId,
            TeacherId = teacher.Id,
            Value = model.Value
        };

        _context.Grades.Add(grade);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // EDIT GET
    [Authorize(Roles = "Admin, Teacher")]
    public async Task<IActionResult> Edit(int id)
    {
        var grade = await _context.Grades.FindAsync(id);

        if (grade == null)
            return NotFound();

        var model = new EditGradeViewModel
        {
            Id = grade.Id,
            Value = grade.Value
        };

        return View(model);
    }

    // EDIT POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin, Teacher")]
    public async Task<IActionResult> Edit(EditGradeViewModel model)
    {
        var grade = await _context.Grades.FindAsync(model.Id);

        if (grade == null)
            return NotFound();

        grade.Value = model.Value;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // DELETE
    [Authorize(Roles = "Admin, Teacher")]
    public async Task<IActionResult> Delete(int id)
    {
        var grade = await _context.Grades.FindAsync(id);

        if (grade == null)
            return NotFound();

        _context.Grades.Remove(grade);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}