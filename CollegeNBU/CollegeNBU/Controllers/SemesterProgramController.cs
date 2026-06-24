using CollegeNBU.Core.Models;
using CollegeNBU.Data.Data;
using CollegeNBU.Web.ViewModels.SemesterProgram;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeNBU.Web.Controllers;

public class SemesterProgramController : Controller
{
    private readonly CollegeDbContext _context;

    public SemesterProgramController(CollegeDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var programs = await _context.SemesterPrograms
            .Include(sp => sp.Department)
            .Include(sp => sp.Courses)
                .ThenInclude(c => c.Course)
            .Select(sp => new SemesterProgramViewModel
            {
                Id = sp.Id,
                Name = sp.Name,
                Semester = sp.Semester,
                DepartmentName = sp.Department.Name,
                Courses = sp.Courses
                    .Select(c => c.Course.Name)
                    .ToList()
            })
            .ToListAsync();

        return View(programs);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Departments = await _context.Departments.ToListAsync();
        ViewBag.Courses = await _context.Courses.ToListAsync();

        CreateSemesterProgramViewModel model = new();

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSemesterProgramViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Departments = await _context.Departments.ToListAsync();
            ViewBag.Courses = await _context.Courses.ToListAsync();

            return View(model);
        }

        var program = new SemesterProgram
        {
            Name = model.Name,
            Semester = model.Semester.ToString(),
            DepartmentId = model.DepartmentId
        };

        _context.SemesterPrograms.Add(program);
        await _context.SaveChangesAsync();

        foreach (var courseId in model.CourseIds)
        {
            _context.SemesterProgramCourses.Add(new SemesterProgramCourse
            {
                SemesterProgramId = program.Id,
                CourseId = courseId
            });
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}