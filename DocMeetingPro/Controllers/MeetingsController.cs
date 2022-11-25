using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DocMeetingPro.Data;
using DocMeetingPro.Models;

namespace DocMeetingPro.Controllers
{
    [Authorize]
    public class MeetingsController : Controller
    {
#nullable enable
        private readonly ApplicationDbContext _context;
        public MeetingsController(ApplicationDbContext context)
        {
            _context = context;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
        public async Task<IActionResult> Index()
        {
            return View(await _context.Meetings.Include(r => r.Saloon).ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.mt = "";

            //ViewBag.Saloon = _context.Saloons.Select(w =>
            //  new SelectListItem
            //  {
            //      Text = w.Name,
            //      Value=w.Id.ToString(),
            //  });
            //Console.WriteLine(ViewBag.Saloon);
            List<SelectListItem> values = (from x in _context.Saloons.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.Name,
                                               Value = x.Id.ToString(),
                                           }).ToList();
            ViewBag.v1 = values;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateMeetingModel meeting)
        {
            var mtCheck = false;
            List<SelectListItem> values = (from x in _context.Saloons.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.Name,
                                               Value = x.Id.ToString(),
                                           }).ToList();
            ViewBag.v1 = values;
            Meeting mt = _context.Meetings.SingleOrDefault(x => x.StartTime == meeting.StartTime);
            Meeting dayCheck, a, b, c;
            List<Meeting> dayChecks = new List<Meeting>();

            //aşağıdaki koşulları kontrol etmek için, daha önce girilmiş olan yıl,ay,gün değerleri ile eşleşmiş olmalı
            dayCheck = _context.Meetings.FirstOrDefault(x => x.StartTime.Date.Year == meeting.StartTime.Date.Year && x.StartTime.Date.Month == meeting.StartTime.Date.Month && x.StartTime.Date.Day == meeting.StartTime.Date.Day);

            foreach (var item in _context.Meetings)
            {
                if (item.StartTime.Date.Year == meeting.StartTime.Date.Year && item.StartTime.Date.Month == meeting.StartTime.Date.Month && item.StartTime.Date.Day == meeting.StartTime.Date.Day)
                {
                    dayChecks.Add(item);
                }
            }
            var suitable = true; var cnt = 0;
            foreach (var item in dayChecks)
            {
                if (item.StartTime != meeting.StartTime)

                {
                    if ((item.EndTime < meeting.StartTime) || (item.StartTime > meeting.EndTime)) //|| (meeting.StartTime > item.EndTime && item.StartTime < meeting.StartTime))
                    {
                        cnt++;
                    }
                    else
                    {
                        return RedirectToAction("Create", "Meetings");
                    }
                }
                else
                {
                    return RedirectToAction("Create", "Meetings");
                }
                //else
                //{
                //    suitable = false;
                //}
            }

            if (cnt == dayChecks.Count)
            {
                _context.Add(new Meeting
                {
                    Name = meeting.Name,
                    SaloonId = meeting.SaloonId,
                    StartTime = meeting.StartTime,
                    EndTime = meeting.EndTime
                });
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Meetings");
            }
            else
            {
                return RedirectToAction("Create", "Meetings");
            }


            //if (dayCheck != null)
            //{
            //    //eklenenin başlangıcı mevcutun başlangıcından küçükse
            //    //a = _context.Meetings.FirstOrDefault(dayCheck => dayCheck.StartTime > meeting.StartTime);
            //    //if (a != null)
            //    if (dayCheck.StartTime > meeting.StartTime)
            //    {
            //        //eklenenin bitişi mevcutun başlangıcından küçük olmalı
            //        //b = _context.Meetings.FirstOrDefault(dayCheck => dayCheck.StartTime > meeting.EndTime);
            //        if (dayCheck.StartTime > meeting.EndTime)
            //        //if(a.Name== b.Name)
            //        {
            //            _context.Add(new Meeting
            //            {
            //                Name = meeting.Name,
            //                SaloonId = meeting.SaloonId,
            //                StartTime = meeting.StartTime,
            //                EndTime = meeting.EndTime
            //            });
            //            await _context.SaveChangesAsync();
            //            return RedirectToAction("Index", "Meetings");
            //        }
            //        else
            //        {
            //            return RedirectToAction("Create", "Meetings");
            //        }
            //    }

            //}


            // b = _context.Meetings.SingleOrDefault(x => x.StartTime < meeting.StartTime);




            ViewBag.mt = mt;
            if (mt != null)
            {
                mtCheck = true;
                ViewBag.mtcheck = true;
                //return RedirectToAction("Create", "Meetings");
                return View();
            }
            if (ModelState.IsValid)
            {
                _context.Add(new Meeting
                {
                    Name = meeting.Name,
                    SaloonId = meeting.SaloonId,
                    StartTime = meeting.StartTime,
                    EndTime = meeting.EndTime
                });
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Meetings");
            }
            return RedirectToAction("Index", "Meetings");
            // return View();



        }
        public async Task<IActionResult> Edit(int? id)
        {
            List<SelectListItem> values = _context.Saloons.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            }).ToList();
            //List<SelectListItem> values = (from x in _context.Saloons
            //                               select new SelectListItem
            //                               {
            //                                   Text = x.Name,
            //                                   Value = x.Id.ToString(),
            //                               }).ToList();
            ViewBag.v1 = values;
            var selectedId = id;
            //var selectedValue = values.Find(u=>u.Id==selectedId);
            //return View();

            if (id == null || _context.Meetings == null)
            {
                return NotFound();
            }

            var meeting = await _context.Meetings.FindAsync(id);
            if (meeting == null)
            {
                return NotFound();
            }
            return View(meeting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartTime,EndTime,SaloonId")] Meeting meeting)
        {
            if (id != meeting.Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
            try
            {
                _context.Update(meeting);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeetingExists(meeting.Id))
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


        //[HttpGet]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    List<SelectListItem> values = _context.Saloons.Select(x => new SelectListItem
        //    {
        //        Text = x.Name,
        //        Value = x.Id.ToString(),
        //    }).ToList();
        //    var exist =await _context.Meetings.Where(x => x.Id == id).ToListAsync();
        //    return View(exist);

        //}

        //[HttpPost]
        //public async Task<IActionResult> Delete(Meeting meeting)
        //{
        //    var exist = await _context.Meetings.Where(x => x.Id == meeting.Id).ToListAsync();
        //    if(exist!=null)
        //    {
        //        _context.Remove(exist);
        //    }
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}



        // GET: Meeting/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Meetings == null)
            {
                return NotFound();
            }

            var meeting = await _context.Meetings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meeting == null)
            {
                return NotFound();
            }

            return View(meeting);
        }

        // POST: Meeting/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Meetings == null)
            {
                return Problem("Entity set 'MetContext.Meeting'  is null.");
            }
            var meeting = await _context.Meetings.FindAsync(id);
            if (meeting != null)
            {
                _context.Meetings.Remove(meeting);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MeetingExists(int id)
        {
            return _context.Meetings.Any(e => e.Id == id);
        }
    }
}
