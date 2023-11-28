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
    public class ModulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;


        public ModulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Modules
        public async Task<IActionResult> Index()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentLoggedInUser = await _context.Modules.Where(m => m.UserId == userid).ToListAsync();

            return View(currentLoggedInUser);
        }
            // GET: Modules/Details/5
            public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Modules == null)
            {
                return NotFound();
            }

            var modules = await _context.Modules
                .FirstOrDefaultAsync(m => m.ModuleId == id);
            if (modules == null)
            {
                return NotFound();
            }

            return View(modules);
        }

        // GET: Modules/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Modules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ModuleId,Code,Name,Credits,ClassHours,Semester,StudyHoursLeft,UserId")] Modules modules)
        {
            if (ModelState.IsValid)
            {

                modules.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                // Calculate remaining study hours
                double remainingStudyHours = Part2StudyHours.Class1.WeeklyStudyHours(modules.Credits, 10, modules.ClassHours);

                // Update the StudyHoursLeft property of the module
                modules.StudyHoursLeft = (int)remainingStudyHours;



                // Assuming you have a DbContext named _context
                _context.Add(modules);
                await _context.SaveChangesAsync();

                // Create a view model to pass data to the view
                var viewModel = new ModuleViewModel
                {
                    Module = modules,
                    RemainingStudyHours = (int)remainingStudyHours
                };

                // Pass the view model to the view
                return View("DisplayHours", viewModel);
            }

            return View(modules);
        }

        // GET: Modules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Modules == null)
            {
                return NotFound();
            }

            var modules = await _context.Modules.FindAsync(id);
            if (modules == null)
            {
                return NotFound();
            }
            return View(modules);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ModuleId,Code,Name,Credits,ClassHours,Semester,StudyHoursLeft,UserId")] Modules modules)
        {
            if (id != modules.ModuleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(modules);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModulesExists(modules.ModuleId))
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
            return View(modules);
        }

        // GET: Modules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Modules == null)
            {
                return NotFound();
            }

            var modules = await _context.Modules
                .FirstOrDefaultAsync(m => m.ModuleId == id);
            if (modules == null)
            {
                return NotFound();
            }

            return View(modules);
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Modules == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Modules'  is null.");
            }
            var modules = await _context.Modules.FindAsync(id);
            if (modules != null)
            {
                _context.Modules.Remove(modules);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModulesExists(int id)
        {
          return (_context.Modules?.Any(e => e.ModuleId == id)).GetValueOrDefault();
        }
    }
}
