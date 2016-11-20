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
        private const string LONG_DATE_FORMAT = "MMMM dd, yyyy h:mm tt";
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
            int delta = DayOfWeek.Monday - today.DayOfWeek;
            if (delta > 0)
                delta -= 7;
            DateTime monday = today.AddDays(delta);
            ViewBag.monday = monday.ToString(LONG_DATE_FORMAT);
            DateTime sunday = monday.AddDays(7);

            //Allow only days this week
            var daysOfTheWeek = @event.Where(e => e.FromDate >= monday && e.FromDate < sunday);

            //add to dictionary
            foreach (var index in daysOfTheWeek.OrderBy(name => name.FromDate).ToList())
            {
                if (index.IsActive)
                {
                    if (Week.ContainsKey(index.FromDate.ToString(LONG_DATE_FORMAT)))
                    {

                        Week[index.FromDate.ToString(LONG_DATE_FORMAT)].Add(index);
                    }
                    else
                    {
                        Week[index.FromDate.ToString(LONG_DATE_FORMAT)] = new List<Event> { index };
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
