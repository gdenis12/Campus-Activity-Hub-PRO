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

        [HttpPost]
        public async Task<IActionResult> Register(int eventId)
        {
            var user = await _userManager.GetUserAsync(User);

            var ev = await _context.Events
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (ev == null)
                return NotFound();

            if (ev.EventDate < DateTime.Now)
                return BadRequest("Подія вже відбулась");

            if (ev.Registrations.Count >= ev.Capacity)
                return BadRequest("Подія заповнена");

            if (ev.Registrations.Any(r => r.UserId == user.Id))
                return BadRequest("Ви вже зареєстровані");

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
