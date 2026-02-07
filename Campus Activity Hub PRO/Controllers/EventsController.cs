using Campus_Activity_Hub_PRO.Data;
using Campus_Activity_Hub_PRO.Models;
using Campus_Activity_Hub_PRO.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

[Authorize]
public class EventsController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public EventsController(AppDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? search)
    {
        var query = _context.Events
            .Include(e => e.Category)
            .Include(e => e.Organizer)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(e =>
                e.Title.Contains(search) ||
                e.Category.Name.Contains(search) ||
                e.Organizer.Name.Contains(search)
            );
        }

        var events = await query
            .OrderByDescending(e => e.EventDate)
            .ToListAsync();

        return View(events);
    }
    [Authorize(Roles = "Student, Admin, Organizer")]
    public async Task<IActionResult> Details(int id)
    {
        var ev = await _context.Events
            .Include(e => e.Category)
            .Include(e => e.Organizer)
            .Include(e => e.Registrations)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (ev == null)
            return NotFound();

        return View(ev);
    }

    [Authorize(Roles = "Organizer, Admin")]
    [HttpGet]
    public IActionResult Create()
    {
        var vm = new EventCreateViewModel
        {
            Categories = _context.Categories
                .Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList()
        };

        return View(vm);
    }

    [Authorize(Roles = "Organizer")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EventCreateViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.Categories = _context.Categories
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList();

            return View(vm);
        }

        var organizer = await _userManager.GetUserAsync(User);

        if (organizer == null)
            return Unauthorized();

        var ev = new Event
        {
            Title = vm.Title,
            Description = vm.Description,
            EventDate = vm.EventDate,
            CategoryId = vm.CategoryId,
            Capacity = vm.Capacity,
            PosterPath = vm.PosterPath,
            OrganizerId = organizer.Id, 
            IsDeleted = false
        };

        _context.Events.Add(ev);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Search(string? search)
    {
        var query = _context.Events
            .Include(e => e.Category)
            .Include(e => e.Organizer)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(e =>
                e.Title.Contains(search) ||
                e.Category.Name.Contains(search) ||
                e.Organizer.Name.Contains(search)
            );
        }

        var events = await query
            .OrderByDescending(e => e.EventDate)
            .ToListAsync();

        return PartialView("_EventsTable", events);
    }

}
