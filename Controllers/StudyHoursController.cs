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
        // Define a private field to store the notification service instance
        private readonly INotyfService _notyf;

        public StudyHoursController(ApplicationDbContext context, INotyfService notyf)
        {
            _context = context;
            // Assign the provided INotyfService instance to the local _notyf field, or throw an ArgumentNullException if null
            _notyf = notyf = notyf ?? throw new ArgumentNullException(nameof(notyf));
        }

        // GET: StudyHours
        public async Task<IActionResult> Index()
        {
            // Retrieve the unique identifier (UserId) for the current user from claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Retrieve study hours data for the current user, including related module information
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
            // Check if the provided studyHours object is of type StudyHours
            if (studyHours is StudyHours)
            {
                // Retrieve the unique identifier (UserId) for the current user from claims
                studyHours.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Add(studyHours);
                await _context.SaveChangesAsync();
                // Check if the notification service (_notyf) is available
                if (_notyf != null)
                {
                    // Display a success notification for successfully tracking the study
                    _notyf.Success("Study successfully Tracked");
                    // Display an additional success notification with instructions to view the bar graph
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
        // Action method to display the study hours graph
        public IActionResult StudyHoursGraph()
        {
            // Retrieve the unique identifier (UserId) for the current user from claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Retrieve study hours data for the current user, including related module information
            var studyHoursData = _context.StudyHours
                .Include(s => s.Module)
                .Where(s => s.UserId == userId) 
                .ToList();
            // Return the StudyHoursGraph view with the retrieved study hours data
            return View(studyHoursData);
        }

        private bool StudyHoursExists(int id)
        {
          return (_context.StudyHours?.Any(e => e.StudyHourId == id)).GetValueOrDefault();
        }
    }
}
