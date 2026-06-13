using CollegeNBU.Data.Data;
using CollegeNBU.Web.ViewModels.Rector;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeNBU.Web.Controllers;

public class RectorController : Controller
{
    private readonly CollegeDbContext _context;

    public RectorController(CollegeDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Dashboard()
    {
        var model = new RectorDashboardViewModel
        {
            TotalStudents = await _context.Students.CountAsync(),
            TotalTeachers = await _context.Teachers.CountAsync(),
            TotalCourses = await _context.Courses.CountAsync(),
            TotalDepartments = await _context.Departments.CountAsync(),

            AverageGrade = await _context.Grades.AnyAsync()
                ? await _context.Grades.AverageAsync(g => g.Value)
                : 0,

            TotalAbsences = await _context.Attendances.CountAsync(a => a.IsAbsent)
        };

        return View(model);
    }

    public async Task<IActionResult> CourseStats()
    {
        var model = await _context.Courses
            .Select(c => new RectorCourseStatsViewModel
            {
                CourseName = c.Name,

                AverageGrade = c.Grades.Any()
                    ? c.Grades.Average(g => g.Value)
                    : 0,

                StudentsCount = c.StudentCourses.Count,

                AbsencesCount = c.Attendances.Count(a => a.IsAbsent)
            })
            .ToListAsync();

        return View(model);
    }
}