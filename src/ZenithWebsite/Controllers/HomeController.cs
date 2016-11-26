using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZenithWebsite.Data;
using Microsoft.EntityFrameworkCore;
using ZenithWebsite.Models.ZenithSocietyModels;

namespace ZenithWebsite.Controllers
{
    public class HomeController : Controller
    {
        private const string LONG_DATE_FORMAT = "MMMM dd, yyyy";
        private ZenithContext db;

        public HomeController(ZenithContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            var @event = db.Event.Include(that => that.Activity);

            Dictionary<String, List<Event>> Week = new Dictionary<String, List<Event>>();

            //Find the monday of this week
            DateTime today = DateTime.Now;
            int deltaToMonday = DayOfWeek.Monday - today.DayOfWeek;
            if (deltaToMonday > 0)
                deltaToMonday -= 7; // Always get this weeks dates
            DateTime monday = today.Date.AddDays(deltaToMonday);
            DateTime nextMonday = monday.AddDays(7);
            ViewBag.StartOfWeek = monday.ToString(LONG_DATE_FORMAT);

            //Allow only days this week
            var daysOfTheWeek = @event.Where(e => e.FromDate >= monday && e.FromDate < nextMonday);

            //add to dictionary
            foreach (var e in daysOfTheWeek.OrderBy(name => name.FromDate).ToList())
            {
                if (e.IsActive)
                {
                    if (Week.ContainsKey(e.FromDate.ToString(LONG_DATE_FORMAT)))
                    {

                        Week[e.FromDate.ToString(LONG_DATE_FORMAT)].Add(e);
                    }
                    else
                    {
                        Week[e.FromDate.ToString(LONG_DATE_FORMAT)] = new List<Event> { e };
                    }
                }
            }

            ViewBag.Week = Week.ToList();

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
