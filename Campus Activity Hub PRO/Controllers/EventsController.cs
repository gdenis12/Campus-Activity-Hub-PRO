using Campus_Activity_Hub_PRO.Data;
using Campus_Activity_Hub_PRO.Models;
using Campus_Activity_Hub_PRO.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Linq;

[Authorize]
public class EventsController : Controller
{
    private readonly AppDbContext _context;

    public EventsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var events = _context.Events
            .Include(e => e.Category)
            .Include(e => e.Organizer)
            .OrderByDescending(e => e.EventDate)
            .ToList();

        return View(events);
    }
    [Authorize(Roles = "Student, Admin")]
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
    public IActionResult Create(EventCreateViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.Categories = _context.Categories
                .Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList();
            return View(vm);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var ev = new Event
        {
            Title = vm.Title,
            Description = vm.Description,
            EventDate = vm.EventDate,
            CategoryId = vm.CategoryId,
            Capacity = vm.Capacity,
            PosterPath = vm.PosterPath,
            OrganizerId = userId,
            IsDeleted = false
        };

        _context.Events.Add(ev);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
}
