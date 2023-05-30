using Exam.Models;
using Exam.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Exam.Areas.AnyarAdmin.Controllers
{
    [Area("AnyarAdmin")]
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signinManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signinManager)
        {
            _userManager = userManager;
            _signinManager = signinManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM user)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Error");
                return View();
            }

            AppUser existed = await _userManager.FindByEmailAsync(user.UsernameOrEmail);
            if(existed==null)
            {
                existed = await _userManager.FindByNameAsync(user.UsernameOrEmail);
                if(existed==null)
                {
                    ModelState.AddModelError(string.Empty, "username,email or password error!");
                    return View();
                }
            }


            var result = await _signinManager.PasswordSignInAsync(existed, user.Password, user.IsRemembered,false);
            if(!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "username,email or password error!");
                return View();
            }

            return RedirectToAction("index", "home", new { area = "" });

        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM newUser)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            AppUser user = new AppUser
            {
                Name = newUser.Name,
                Surname = newUser.Surname,
                Email = newUser.Email,
                UserName = newUser.Username
            };

            var result= await _userManager.CreateAsync(user,newUser.Password);
            if(!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                return View();
            }
            await _signinManager.SignInAsync(user, false);
            return RedirectToAction("index", "home", new {area=""});
        }

        public async Task<IActionResult> Logout()
        {
            await _signinManager.SignOutAsync();
            return RedirectToAction("index", "home", new { area = "" });

        }

    }
}
