using Campus_Activity_Hub_PRO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Campus_Activity_Hub_PRO_comand_second.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            
            var eventsByCategory = await _context.Events
                .Where(e => !e.IsDeleted)
                .Include(e => e.Category)
                .GroupBy(e => e.Category.Name)
                .Select(g => new
                {
                    Category = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            
            var topEvents = await _context.Registrations
                .Include(r => r.Event)
                .GroupBy(r => r.Event.Title)
                .Select(g => new
                {
                    Event = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToListAsync();

            
            var registrations = await _context.Registrations
                .AsNoTracking()
                .ToListAsync();

            var registrationsByMonth = registrations
                .GroupBy(r => new { r.RegistrationDate.Year, r.RegistrationDate.Month })
                .Select(g => new
                {
                    Month = $"{g.Key.Month}/{g.Key.Year}", 
                    Count = g.Count()
                })
                .OrderBy(x => x.Month)
                .ToList();

            
            ViewBag.EventsByCategory = eventsByCategory;
            ViewBag.TopEvents = topEvents;
            ViewBag.RegistrationsByMonth = registrationsByMonth;

            return View();
        }
    }
}
