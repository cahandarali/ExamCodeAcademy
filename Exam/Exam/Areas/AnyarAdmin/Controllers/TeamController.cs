using Exam.DAL;
using Exam.Models;
using Exam.Utilities.Extentions;
using Exam.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exam.Areas.AnyarAdmin.Controllers
{
    [Area("AnyarAdmin")]
    [AutoValidateAntiforgeryToken]
    public class TeamController : Controller
    {

        private readonly AppDBContext _context;
        private readonly IWebHostEnvironment _env;

        public TeamController(AppDBContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index(int page=0)
        {

            List<TeamMember> members = _context.TeamMembers.Skip(page*5).Take(5).Include(p=>p.Position).ToList();
            PaginateVM<TeamMember> paginateVM = new PaginateVM<TeamMember>
            {
                CurrentPage = page,
                TotalPage = Math.Ceiling((decimal)_context.TeamMembers.Count() / 5),
                Item = members
            };
            return View(paginateVM);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Positions = await _context.Positions.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TeamMember newMember)
        {
            ViewBag.Positions = await _context.Positions.ToListAsync();
            if (ModelState.IsValid)
            {
                return View();
            }
            if (newMember.PositionId == 0)
            {
                ModelState.AddModelError("PositionId", "Zehmet olmasa Profession secin");
                return View();
            }




            bool result = await _context.Positions.AnyAsync(p => p.Id == newMember.PositionId);
            if (!result)
            {
                ModelState.AddModelError("PositionId", "Bu idli ixtisas yoxdu");
                return View();
            }

            if (newMember.Photo==null)
            {
                ModelState.AddModelError("Photo", "Photo can`t be null!");
                return View();
            }

            if (!newMember.Photo.CheckFileSize(20000))
            {
                ModelState.AddModelError("Photo", "File hecmi uygun deyil");
                return View();
            }

            if (!newMember.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "File tipi uygun deyil");
                return View();
            }

           

            newMember.Image = await newMember.Photo.CreateFileAsync(_env.WebRootPath, "assets/img/team");

            await _context.TeamMembers.AddAsync(newMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Positions = await _context.Positions.ToListAsync();
            if (id == null || id < 1) return BadRequest();
            TeamMember existed = await _context.TeamMembers.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();


            return View(existed);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, TeamMember member)
        {
            ViewBag.Positions = await _context.Positions.ToListAsync();

            if (id == null || id < 1) return BadRequest();
            TeamMember existed = await _context.TeamMembers.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null) return NotFound();

            bool result = await _context.Positions.AnyAsync(p => p.Id == member.PositionId);
            if (!result)
            {
                ModelState.AddModelError("PositionId", "Bu idli ixtisas yoxdu");
                return View(existed);
            }

            if (member.Photo != null)
            {
                if (!member.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File tipi uygun deyil");
                    return View();
                }

                if (!member.Photo.CheckFileSize(2000))
                {
                    ModelState.AddModelError("Photo", "File hecmi 200kbdan boyuk olmamalidi!");
                    return View();
                }

                existed.Image.DeleteFile(_env.WebRootPath, "assets/img/team");
                existed.Image = await member.Photo.CreateFileAsync(_env.WebRootPath, "assets/img/team");
            }



            existed.Name = member.Name;
            existed.Surname = member.Surname;
            existed.Description = member.Description;
            existed.PositionId = member.PositionId;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id < 1)
            {
                return BadRequest();
            }

            TeamMember member = await _context.TeamMembers.FirstOrDefaultAsync(s => s.Id == id);

            if (member == null) return NotFound();


            member.Image.DeleteFile(_env.WebRootPath, "assets/img/team");
            _context.TeamMembers.Remove(member);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
