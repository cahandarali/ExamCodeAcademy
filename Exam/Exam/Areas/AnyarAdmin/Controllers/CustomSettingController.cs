using Exam.DAL;
using Exam.Models;
using Exam.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exam.Areas.AnyarAdmin.Controllers
{
    [Area("AnyarAdmin")]
    [AutoValidateAntiforgeryToken]
    public class CustomSettingController : Controller
    {
        private readonly AppDBContext _context;

        public CustomSettingController(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page=0)
        {
            List<Setting> sets = await _context.Settings.Skip(page*5).Take(5).ToListAsync();
            PaginateVM<Setting> paginateVM = new PaginateVM<Setting>
            {
                CurrentPage = page,
                TotalPage = Math.Ceiling((decimal)_context.TeamMembers.Count() / 5),
                Item = sets
            };
            return View(paginateVM);
        }


        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id < 1) return BadRequest();

            var existed = await _context.Settings.FirstOrDefaultAsync(s => s.Id == id);
            if (existed == null) return NotFound();

            return View(existed);
        }
        [HttpPost]

        public async Task<IActionResult> Update(Setting setting)
        {
            var existed = await _context.Settings.FirstOrDefaultAsync(s => s.Id == setting.Id);
            if (existed == null) return NotFound();

            existed.Value = setting.Value;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
