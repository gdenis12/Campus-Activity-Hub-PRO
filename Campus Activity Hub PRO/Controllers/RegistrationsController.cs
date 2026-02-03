using Campus_Activity_Hub_PRO.Data;
using Campus_Activity_Hub_PRO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Campus_Activity_Hub_PRO.Controllers
{
    [Authorize(Roles = "Student")]
    public class RegistrationsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public RegistrationsController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> My()
        {
            var userId = _userManager.GetUserId(User);

            var registrations = await _context.Registrations
                .Include(r => r.Event)
                    .ThenInclude(e => e.Category)
                .Include(r => r.Event)
                    .ThenInclude(e => e.Organizer)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.RegistrationDate)
                .ToListAsync();

            return View(registrations);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(int eventId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var ev = await _context.Events
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (ev == null)
                return NotFound();

            if (ev.EventDate < DateTime.Now)
                return BadRequest("The event has already taken place");

            if (ev.Registrations.Count >= ev.Capacity)
                return BadRequest("The event is full");

            if (ev.Registrations.Any(r => r.UserId == user.Id))
                return BadRequest("You are already registered");

            var registration = new Registration
            {
                EventId = ev.Id,
                UserId = user.Id,
                RegistrationDate = DateTime.Now
            };

            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Events", new { id = ev.Id });
        }
    }
}