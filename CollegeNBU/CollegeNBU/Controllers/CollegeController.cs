using CollegeNBU.Core.Models;
using CollegeNBU.Data.Data;
using CollegeNBU.ViewModels.College;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeNBU.Web.Controllers;

public class CollegeController : Controller
{
    private readonly CollegeDbContext _context;

    public CollegeController(CollegeDbContext context)
    {
        _context = context;
    }

    // LIST
    public async Task<IActionResult> Index()
    {
        var colleges = await _context.Colleges
            .Select(c => new CollegeViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Address = c.Address
            })
            .ToListAsync();

        return View(colleges);
    }

    // CREATE (GET)
    public IActionResult Create()
    {
        return View();
    }

    // CREATE (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CollegeViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var college = new College
        {
            Name = model.Name,
            Address = model.Address
        };

        _context.Colleges.Add(college);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // EDIT (GET)
    public async Task<IActionResult> Edit(int id)
    {
        var college = await _context.Colleges.FindAsync(id);

        if (college == null)
            return NotFound();

        var model = new CollegeViewModel
        {
            Id = college.Id,
            Name = college.Name,
            Address = college.Address
        };

        return View(model);
    }

    // EDIT (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CollegeViewModel model)
    {
        var college = await _context.Colleges.FindAsync(model.Id);

        if (college == null)
            return NotFound();

        college.Name = model.Name;
        college.Address = model.Address;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // DELETE
    public async Task<IActionResult> Delete(int id)
    {
        var college = await _context.Colleges.FindAsync(id);

        if (college == null)
            return NotFound();

        _context.Colleges.Remove(college);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}