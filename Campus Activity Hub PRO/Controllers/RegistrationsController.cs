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
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> RegisterAjax([FromBody] int eventId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Json(new { success = false, message = "Unauthorized" });

            var ev = await _context.Events
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (ev == null)
                return Json(new { success = false, message = "Event not found" });

            if (ev.EventDate < DateTime.Now)
                return Json(new { success = false, message = "The event has already taken place" });

            if (ev.Registrations.Count >= ev.Capacity)
                return Json(new { success = false, message = "The event is full" });

            if (ev.Registrations.Any(r => r.UserId == user.Id))
                return Json(new { success = false, message = "You are already registered" });

            _context.Registrations.Add(new Registration
            {
                EventId = ev.Id,
                UserId = user.Id,
                RegistrationDate = DateTime.Now
            });

            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

    }
}