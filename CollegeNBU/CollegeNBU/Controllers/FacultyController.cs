using CollegeNBU.Core.Models;
using CollegeNBU.Data.Data;
using CollegeNBU.Web.ViewModels.Faculty;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeNBU.Web.Controllers;

[Authorize(Roles = "Admin")]
public class FacultyController : Controller
{
    private readonly CollegeDbContext _context;

    public FacultyController(CollegeDbContext context)
    {
        _context = context;
    }

    // LIST
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var faculties = await _context.Faculties
            .Include(f => f.College)
            .Select(f => new FacultyViewModel
            {
                Id = f.Id,
                Name = f.Name,
                CollegeId = f.CollegeId,
                CollegeName = f.College.Name
            })
            .ToListAsync();

        return View(faculties);
    }

    // CREATE GET
    public async Task<IActionResult> Create()
    {
        ViewBag.Colleges = await _context.Colleges.ToListAsync();
        return View();
    }

    // CREATE POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateFacultyViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Colleges = await _context.Colleges.ToListAsync();
            return View(model);
        }

        var faculty = new Faculty
        {
            Name = model.Name,
            CollegeId = model.CollegeId
        };

        _context.Faculties.Add(faculty);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // EDIT GET
    public async Task<IActionResult> Edit(int id)
    {
        var faculty = await _context.Faculties.FindAsync(id);

        if (faculty == null)
            return NotFound();

        var model = new EditFacultyViewModel
        {
            Id = faculty.Id,
            Name = faculty.Name,
            CollegeId = faculty.CollegeId
        };

        ViewBag.Colleges = await _context.Colleges.ToListAsync();

        return View(model);
    }

    // EDIT POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditFacultyViewModel model)
    {
        var faculty = await _context.Faculties.FindAsync(model.Id);

        if (faculty == null)
            return NotFound();

        faculty.Name = model.Name;
        faculty.CollegeId = model.CollegeId;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // DELETE
    public async Task<IActionResult> Delete(int id)
    {
        var faculty = await _context.Faculties.FindAsync(id);

        if (faculty == null)
            return NotFound();

        _context.Faculties.Remove(faculty);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}