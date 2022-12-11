using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Appointment_Scheduler.Data;
using Appointment_Scheduler.Models;

namespace Appointment_Scheduler.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public static Dictionary<string, int> numOfApptDaily = new Dictionary<string, int>();
        public AppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }
       

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            
            
            CountDailyAppts();
            return View(await _context.Appointment.ToListAsync());
        }

        public void CountDailyAppts()
        {
            //Check for within the next 7 days including today
            //count the number for each day
            try
            {
                numOfApptDaily.Clear();
                numOfApptDaily["Monday"] = 0;
                numOfApptDaily["Tuesday"] = 0;
                numOfApptDaily["Wednesday"] = 0;
                numOfApptDaily["Thursday"] = 0;
                numOfApptDaily["Friday"] = 0;
                numOfApptDaily["Saturday"] = 0;
                numOfApptDaily["Sunday"] = 0;
                List<String> list = new List<String>();
                foreach(Appointment appointment in _context.Appointment)
                {
                    if(DateTime.Parse(appointment.ScheduledDate)>=DateTime.Today &&
                        DateTime.Parse(appointment.ScheduledDate) < DateTime.Today.AddDays(7))
                    {
                        list.Add(DateTime.Parse(appointment.ScheduledDate).DayOfWeek + "");
                    }
                }
                foreach(var day in list)
                {
                    if (numOfApptDaily.ContainsKey(day)) numOfApptDaily[day]++;
                }
                //numOfApptDaily["Monday"] = list.Aggregate(a => a=="Monday").Count();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Appointment == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,ScheduledDate,StartTime,EndTime,Phone")] Appointment appointment)
        {
            
            if (ModelState.IsValid && CheckRestritions(appointment))
            {
                appointment.Created = DateTime.Now;
                appointment.StartTime = DateTime.Parse(appointment.ScheduledDate) + appointment.StartTime.TimeOfDay;
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(appointment);
        }

        public bool CheckRestritions(Appointment app)
        {
            //TODO fix restrictions on phone number
            var listOfAppAtSameTime = _context.Appointment.Where(a => a.ScheduledDate == app.ScheduledDate
            && ((a.StartTime >= app.StartTime && a.EndTime < app.EndTime)
            || (a.StartTime <= app.StartTime && a.EndTime >= app.EndTime)
            || (a.EndTime > app.StartTime && a.EndTime <= app.EndTime))).ToList();


            if (listOfAppAtSameTime.Count >= 3) { 

                return false; }
            else{
                return true;
            }
            
        }


        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Appointment == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,ScheduledDate,StartTime,EndTime,Phone")] Appointment appointment)
        {

            appointment.Id = id;
            appointment.Created = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
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
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Appointment == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Appointment == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Appointment'  is null.");
            }
            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointment.Remove(appointment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
          return _context.Appointment.Any(e => e.Id == id);
        }
    }
}
