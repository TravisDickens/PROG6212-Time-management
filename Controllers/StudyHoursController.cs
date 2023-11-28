using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TimeManagementPOE.Data;
using TimeManagementPOE.Models;

namespace TimeManagementPOE.Controllers
{
    public class StudyHoursController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyf;

        public StudyHoursController(ApplicationDbContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf = notyf ?? throw new ArgumentNullException(nameof(notyf));
        }

        // GET: StudyHours
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentLoggedInUser = await _context.StudyHours
                .Where(sh => sh.UserId == userId)
                .Include(s => s.Module)
                .ToListAsync();

            return View(currentLoggedInUser);
        }

        // GET: StudyHours/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.StudyHours == null)
            {
                return NotFound();
            }

            var studyHours = await _context.StudyHours
                .Include(s => s.Module)
                .FirstOrDefaultAsync(m => m.StudyHourId == id);
            if (studyHours == null)
            {
                return NotFound();
            }

            return View(studyHours);
        }

        // GET: StudyHours/Create
        public IActionResult Create()
        {
           
            // Get the currently logged-in user ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve modules for the current user
            var modules = _context.Modules
                .Where(m => m.UserId == userId)
                .ToList();

            // Create a SelectList for the dropdown
            ViewBag.ModuleId = new SelectList(modules, "ModuleId", "Name");
            return View();
        }

        // POST: StudyHours/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudyHourId,ModuleId,Date,Hours,UserId")] StudyHours studyHours)
        {
            if (studyHours is StudyHours)
            {
                studyHours.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Add(studyHours);
                await _context.SaveChangesAsync();
                if (_notyf != null)
                {
                    _notyf.Success("Study successfully Tracked");
                    _notyf.Success("You may now view the bar graph");
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleId", studyHours.ModuleId);
            return View(studyHours);
        }

        // GET: StudyHours/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.StudyHours == null)
            {
                return NotFound();
            }

            var studyHours = await _context.StudyHours.FindAsync(id);
            if (studyHours == null)
            {
                return NotFound();
            }
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleId", studyHours.ModuleId);
            return View(studyHours);
        }

        // POST: StudyHours/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudyHourId,ModuleId,Date,Hours,UserId")] StudyHours studyHours)
        {
            if (id != studyHours.StudyHourId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studyHours);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudyHoursExists(studyHours.StudyHourId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleId", studyHours.ModuleId);
            return View(studyHours);
        }

        // GET: StudyHours/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.StudyHours == null)
            {
                return NotFound();
            }

            var studyHours = await _context.StudyHours
                .Include(s => s.Module)
                .FirstOrDefaultAsync(m => m.StudyHourId == id);
            if (studyHours == null)
            {
                return NotFound();
            }

            return View(studyHours);
        }

        // POST: StudyHours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.StudyHours == null)
            {
                return Problem("Entity set 'ApplicationDbContext.StudyHours'  is null.");
            }
            var studyHours = await _context.StudyHours.FindAsync(id);
            if (studyHours != null)
            {
                _context.StudyHours.Remove(studyHours);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //Get infomation for the graph
        public IActionResult StudyHoursGraph()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var studyHoursData = _context.StudyHours
                .Include(s => s.Module)
                .Where(s => s.UserId == userId) // Adjust the property name accordingly
                .ToList();

            return View(studyHoursData);
        }

        private bool StudyHoursExists(int id)
        {
          return (_context.StudyHours?.Any(e => e.StudyHourId == id)).GetValueOrDefault();
        }
    }
}
