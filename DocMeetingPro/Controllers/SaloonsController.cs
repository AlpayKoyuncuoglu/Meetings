using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using DocMeetingPro.Data;
using DocMeetingPro.Models;

namespace DocMeetingPro.Controllers
{
    [Authorize]
    public class SaloonsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SaloonsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Saloons.ToListAsync());
        }
        public IActionResult Create()
        {
            ViewBag.sln = "";

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Saloon saloon)
        {
            if (saloon.Name == null)
            {
                return RedirectToAction("Index", "Saloons");
            }

            Saloon sln = _context.Saloons.SingleOrDefault(x => x.Name.ToLower() == saloon.Name.ToLower());

            ViewBag.sln = sln?.Name;


            if (sln?.Name != null)
            {
                return View();
            }
            else if (ModelState.IsValid)
            {
                _context.Add(new Saloon
                {
                    Name = saloon.Name,
                    Id = saloon.Id,
                });
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Saloons");
            }
            //return View();
            return RedirectToAction("Index", "Saloons");


        }


        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null || _context.Saloons == null)
            {
                return NotFound();
            }

            var saloon = await _context.Saloons.FindAsync(id);
            if (saloon == null)
            {
                return NotFound();
            }
            return View(saloon);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Saloon saloon)
        {
            if (id != saloon.Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
            try
            {
                _context.Update(saloon);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaloonExists(saloon.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
            //}
            //return View(meeting);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Saloons == null)
            {
                return NotFound();
            }

            var saloon = await _context.Saloons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (saloon == null)
            {
                return NotFound();
            }

            return View(saloon);
        }

        // POST: Meeting/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Saloons == null)
            {
                return Problem("Entity set 'MetContext.Meeting'  is null.");
            }
            var saloon = await _context.Saloons.FindAsync(id);
            if (saloon != null)
            {
                _context.Saloons.Remove(saloon);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SaloonExists(int id)
        {
            return _context.Saloons.Any(e => e.Id == id);
        }





    }
}
