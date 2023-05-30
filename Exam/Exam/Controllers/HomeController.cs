using Exam.DAL;
using Exam.Models;
using Exam.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Exam.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDBContext _context;

        public HomeController(AppDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<TeamMember> teammembers = _context.TeamMembers.Include(p=>p.Position).ToList();
            List<Slider> slidersss=_context.Sliders.ToList();
            List<IconBox> iconBoxes = _context.IconBoxes.ToList();
            HomeVM homeVM = new HomeVM
            {
                BoxesIcon= iconBoxes,
                members = teammembers,
                sliders = slidersss
            };
            return View(homeVM);
        }


    }
}