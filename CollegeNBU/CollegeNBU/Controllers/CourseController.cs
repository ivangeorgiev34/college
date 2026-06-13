using CollegeNBU.Core.Models;
using CollegeNBU.Data.Data;
using CollegeNBU.Web.ViewModels.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeNBU.Web.Controllers;

public class CourseController : Controller
{
    private readonly CollegeDbContext _context;

    public CourseController(CollegeDbContext context)
    {
        _context = context;
    }

    // LIST
    public async Task<IActionResult> Index()
    {
        var courses = await _context.Courses
            .Include(c => c.Department)
            .Include(c => c.Teacher)
            .Select(c => new CourseViewModel
            {
                Id = c.Id,
                Name = c.Name,
                DepartmentName = c.Department.Name,
                TeacherName = c.Teacher.FirstName + " " + c.Teacher.LastName
            })
            .ToListAsync();

        return View(courses);
    }

    // CREATE GET
    public async Task<IActionResult> Create()
    {
        ViewBag.Departments = await _context.Departments.ToListAsync();
        ViewBag.Teachers = await _context.Teachers.ToListAsync();

        return View();
    }

    // CREATE POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCourseViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Departments = await _context.Departments.ToListAsync();
            ViewBag.Teachers = await _context.Teachers.ToListAsync();
            return View(model);
        }

        var course = new Course
        {
            Name = model.Name,
            DepartmentId = model.DepartmentId,
            TeacherId = model.TeacherId
        };

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // EDIT GET
    public async Task<IActionResult> Edit(int id)
    {
        var course = await _context.Courses.FindAsync(id);

        if (course == null)
            return NotFound();

        var model = new EditCourseViewModel
        {
            Id = course.Id,
            Name = course.Name,
            DepartmentId = course.DepartmentId,
            TeacherId = course.TeacherId
        };

        ViewBag.Departments = await _context.Departments.ToListAsync();
        ViewBag.Teachers = await _context.Teachers.ToListAsync();

        return View(model);
    }

    // EDIT POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditCourseViewModel model)
    {
        var course = await _context.Courses.FindAsync(model.Id);

        if (course == null)
            return NotFound();

        course.Name = model.Name;
        course.DepartmentId = model.DepartmentId;
        course.TeacherId = model.TeacherId;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // DELETE
    public async Task<IActionResult> Delete(int id)
    {
        var course = await _context.Courses.FindAsync(id);

        if (course == null)
            return NotFound();

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}