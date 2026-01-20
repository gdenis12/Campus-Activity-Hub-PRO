using Campus_Activity_Hub_PRO.Models.Auth;
using Campus_Activity_Hub_PRO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Campus_Activity_Hub_PRO.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register()
            => View(new RegisterVm());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVm vm, CancellationToken ct)
        {

            var allowedRoles = new[] { "Student", "Organizer" };
            if (!allowedRoles.Contains(vm.Role))
                ModelState.AddModelError(nameof(vm.Role), "Invalid role");

            if (!ModelState.IsValid) return View(vm);


            foreach (var r in new[] { "Admin", "Organizer", "Student" })
                if (!await _roleManager.RoleExistsAsync(r))
                    await _roleManager.CreateAsync(new IdentityRole(r));

            var user = new AppUser
            {
                UserName = vm.Email,
                Email = vm.Email,
                Name = vm.Name
            };

            var createRes = await _userManager.CreateAsync(user, vm.Password);
            if (!createRes.Succeeded)
            {
                foreach (var e in createRes.Errors)
                    ModelState.AddModelError("", e.Description);

                return View(vm);
            }

            await _userManager.AddToRoleAsync(user, vm.Role);

            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Events");
        }


        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password, string? returnUrl = null)
        {
            var res = await _signInManager.PasswordSignInAsync(email, password, false, lockoutOnFailure: false);
            if (!res.Succeeded)
            {
                ModelState.AddModelError("", "Invalid email or password");
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            var user = await _userManager.FindByEmailAsync(email);

            if (user != null && await _userManager.IsInRoleAsync(user, "Admin"))
                return RedirectToAction("Dashboard", "Admin");

            return RedirectToAction("Index", "Events");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Events");
        }

        [HttpGet]
        public IActionResult Denied() => View();
    }
}
