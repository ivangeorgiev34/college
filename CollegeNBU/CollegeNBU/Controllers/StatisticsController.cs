using CollegeNBU.Data.Data;
using CollegeNBU.Web.ViewModels.Statistics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeNBU.Web.Controllers;

public class StatisticsController : Controller
{
    private readonly CollegeDbContext _context;

    public StatisticsController(CollegeDbContext context)
    {
        _context = context;
    }

    // COLLEGE OVERVIEW (Admin / Rector)
    public async Task<IActionResult> College()
    {
        var model = new CollegeStatisticsViewModel
        {
            TotalStudents = await _context.Students.CountAsync(),
            TotalTeachers = await _context.Teachers.CountAsync(),
            TotalCourses = await _context.Courses.CountAsync(),

            OverallAverageGrade = await _context.Grades.AnyAsync()
                ? await _context.Grades.AverageAsync(g => g.Value)
                : 0,

            TotalAbsences = await _context.Attendances
                .CountAsync(a => a.IsAbsent)
        };

        return View(model);
    }

    // COURSE STATISTICS
    public async Task<IActionResult> Courses()
    {
        var model = await _context.Courses
            .Select(c => new CourseStatisticsViewModel
            {
                CourseName = c.Name,
                AverageGrade = c.Grades.Any()
                    ? c.Grades.Average(g => g.Value)
                    : 0,
                TotalGrades = c.Grades.Count
            })
            .ToListAsync();

        return View(model);
    }

    // TEACHER STATISTICS
    public async Task<IActionResult> Teachers()
    {
        var model = await _context.Teachers
            .Select(t => new TeacherStatisticsViewModel
            {
                TeacherName = t.FirstName + " " + t.LastName,
                CoursesCount = t.Courses.Count,
                AverageGrade = t.Courses
                    .SelectMany(c => c.Grades)
                    .Any()
                        ? t.Courses.SelectMany(c => c.Grades).Average(g => g.Value)
                        : 0,
                TotalAbsences = t.Attendances.Count(a => a.IsAbsent)
            })
            .ToListAsync();

        return View(model);
    }
}