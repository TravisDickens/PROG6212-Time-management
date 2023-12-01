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
    public class SemestersController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;
        //declares a private readonly field of INotyfService
        private readonly INotyfService _notyf;

        public SemestersController(ApplicationDbContext context, INotyfService notyf)
        {
            _context = context;
            //makes sure Inotyf is not null
            _notyf = notyf ?? throw new ArgumentNullException(nameof(notyf));
        }

        // GET: Semesters
        public async Task<IActionResult> Index()
        {
            // Retrieve the user ID of the currently logged-in user
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Fetch a list of semesters associated with the user ID from the database
            var currentUser = await _context.Semesters
                                      .Where(s => s.UserId == userid) // Filter semesters based on user ID
                                      .ToListAsync();//Convert the result to a list asynchronously


            return View(currentUser);
        }

        // GET: Semesters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Semesters == null)
            {
                return NotFound();
            }

            var semesters = await _context.Semesters
                .FirstOrDefaultAsync(m => m.SemesterId == id);
            if (semesters == null)
            {
                return NotFound();
            }

            return View(semesters);
        }

        // GET: Semesters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Semesters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SemesterId,Name,Weeks,StartDate,EndDate,UserId")] Semesters semesters)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the user ID of the currently logged-in user
                semesters.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _context.Add(semesters);
                await _context.SaveChangesAsync();
                // Check if the notification service is available
                if (_notyf != null)
                {
                    // Display success messages using the notification service
                    _notyf.Success("Semester successfully added");
                    _notyf.Success("You may now add modules");
                }
                // Redirect to the Index action of the SemestersController
                return RedirectToAction(nameof(Index));
            }
            return View(semesters);
        }

        // GET: Semesters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Semesters == null)
            {
                return NotFound();
            }

            var semesters = await _context.Semesters.FindAsync(id);
            if (semesters == null)
            {
                return NotFound();
            }
            return View(semesters);
        }

        // POST: Semesters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SemesterId,Name,Weeks,StartDate,EndDate,UserId")] Semesters semesters)
        {
            if (id != semesters.SemesterId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(semesters);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SemestersExists(semesters.SemesterId))
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
            return View(semesters);
        }

        // GET: Semesters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Semesters == null)
            {
                return NotFound();
            }

            var semesters = await _context.Semesters
                .FirstOrDefaultAsync(m => m.SemesterId == id);
            if (semesters == null)
            {
                return NotFound();
            }

            return View(semesters);
        }

        // POST: Semesters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Semesters == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Semesters'  is null.");
            }
            var semesters = await _context.Semesters.FindAsync(id);
            if (semesters != null)
            {
                _context.Semesters.Remove(semesters);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SemestersExists(int id)
        {
          return (_context.Semesters?.Any(e => e.SemesterId == id)).GetValueOrDefault();
        }
    }
}
