using CollegeNBU.Core.Models;
using CollegeNBU.Data.Data;
using CollegeNBU.Web.ViewModels.Student;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeNBU.Web.Controllers;

public class StudentController : Controller
{
    private readonly CollegeDbContext _context;

    public StudentController(CollegeDbContext context)
    {
        _context = context;
    }

    // LIST
    public async Task<IActionResult> Index()
    {
        var students = await _context.Students
            .Include(s => s.Department)
            .Select(s => new StudentViewModel
            {
                Id = s.Id,
                FullName = s.FirstName + " " + s.LastName,
                Email = s.Email,
                DepartmentName = s.Department.Name
            })
            .ToListAsync();

        return View(students);
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
    public async Task<IActionResult> Create(CreateStudentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Departments = await _context.Departments.ToListAsync();
            return View(model);
        }

        var student = new Student
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            DepartmentId = model.DepartmentId
        };

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // EDIT GET
    public async Task<IActionResult> Edit(int id)
    {
        var student = await _context.Students.FindAsync(id);

        if (student == null)
            return NotFound();

        var model = new EditStudentViewModel
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Email = student.Email,
            DepartmentId = student.DepartmentId
        };

        ViewBag.Departments = await _context.Departments.ToListAsync();

        return View(model);
    }

    // EDIT POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditStudentViewModel model)
    {
        var student = await _context.Students.FindAsync(model.Id);

        if (student == null)
            return NotFound();

        student.FirstName = model.FirstName;
        student.LastName = model.LastName;
        student.Email = model.Email;
        student.DepartmentId = model.DepartmentId;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // DELETE
    public async Task<IActionResult> Delete(int id)
    {
        var student = await _context.Students.FindAsync(id);

        if (student == null)
            return NotFound();

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // ENROLL GET
    public async Task<IActionResult> Enroll(int id)
    {
        var student = await _context.Students.FindAsync(id);

        if (student == null)
            return NotFound();

        ViewBag.Courses = await _context.Courses.ToListAsync();

        var model = new EnrollStudentViewModel
        {
            StudentId = id,
            SelectedCourseIds = await _context.StudentCourses
                .Where(sc => sc.StudentId == id)
                .Select(sc => sc.CourseId)
                .ToListAsync()
        };

        return View(model);
    }

    // ENROLL POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Enroll(EnrollStudentViewModel model)
    {
        var existing = _context.StudentCourses
            .Where(sc => sc.StudentId == model.StudentId);

        _context.StudentCourses.RemoveRange(existing);

        foreach (var courseId in model.SelectedCourseIds)
        {
            _context.StudentCourses.Add(new StudentCourse
            {
                StudentId = model.StudentId,
                CourseId = courseId
            });
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

}