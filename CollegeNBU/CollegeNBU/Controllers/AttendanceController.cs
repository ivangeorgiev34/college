using CollegeNBU.Core.Models;
using CollegeNBU.Data.Data;
using CollegeNBU.Web.ViewModels.Attendance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CollegeNBU.Web.Controllers;

public class AttendanceController : Controller
{
    private readonly CollegeDbContext _context;

    public AttendanceController(CollegeDbContext context)
    {
        _context = context;
    }

    // LIST
    public async Task<IActionResult> Index()
    {
        var attendances = await _context.Attendances
            .Include(a => a.Student)
            .Include(a => a.Course)
            .Include(a => a.Teacher)
            .Select(a => new AttendanceViewModel
            {
                Id = a.Id,
                StudentName = a.Student.FirstName + " " + a.Student.LastName,
                CourseName = a.Course.Name,
                IsAbsent = a.IsAbsent,
                TeacherName = a.Teacher.FirstName + " " + a.Teacher.LastName
            })
            .ToListAsync();

        return View(attendances);
    }

    // CREATE GET
    public async Task<IActionResult> Create()
    {
        ViewBag.Students = await _context.Students.ToListAsync();
        ViewBag.Courses = await _context.Courses.ToListAsync();

        return View();
    }

    // CREATE POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateAttendanceViewModel model)
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

        // проверка: teacher може да маркира само свои курсове
        var course = await _context.Courses
            .FirstOrDefaultAsync(c => c.Id == model.CourseId && c.TeacherId == teacher.Id);

        if (course == null)
            return Forbid();

        var attendance = new Attendance
        {
            StudentId = model.StudentId,
            CourseId = model.CourseId,
            TeacherId = teacher.Id,
            IsAbsent = model.IsAbsent
        };

        _context.Attendances.Add(attendance);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // EDIT GET
    public async Task<IActionResult> Edit(int id)
    {
        var attendance = await _context.Attendances.FindAsync(id);

        if (attendance == null)
            return NotFound();

        var model = new EditAttendanceViewModel
        {
            Id = attendance.Id,
            IsAbsent = attendance.IsAbsent
        };

        return View(model);
    }

    // EDIT POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditAttendanceViewModel model)
    {
        var attendance = await _context.Attendances.FindAsync(model.Id);

        if (attendance == null)
            return NotFound();

        attendance.IsAbsent = model.IsAbsent;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // DELETE
    public async Task<IActionResult> Delete(int id)
    {
        var attendance = await _context.Attendances.FindAsync(id);

        if (attendance == null)
            return NotFound();

        _context.Attendances.Remove(attendance);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}