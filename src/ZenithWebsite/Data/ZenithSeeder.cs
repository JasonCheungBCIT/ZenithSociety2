using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZenithWebsite.Data;
using ZenithWebsite.Models.ZenithSocietyModels;

namespace ZenithWebsite.Data
{
    public class ZenithSeeder
    {
        public static void Seed(ZenithContext db)
        {           
            if (!db.Activity.Any())
            {
                db.Activity.AddRange(getActivities());
                db.SaveChanges();   // Necessary for Event seeding
            }
            if (!db.Event.Any())
            {
                db.Event.AddRange(getEvents(db));
                db.SaveChanges();
            }
        }

        private static List<Activity> getActivities()
        {
            List<Activity> activities = new List<Activity>();
            activities.Add(new Activity()
            {
                ActivityId = 1,
                ActivityDescription = "Young ladies cooking lessons",
                CreationDate = new DateTime(2015, 12, 30, 23, 59, 59)
            });
            activities.Add(new Activity()
            {
                ActivityId = 2,
                ActivityDescription = "Youth choir practice",
                CreationDate = new DateTime(2015, 12, 30, 23, 59, 59)
            });
            activities.Add(new Activity()
            {
                ActivityId = 3,
                ActivityDescription = "Bingo Tournament",
                CreationDate = new DateTime(2015, 12, 30, 23, 59, 59)
            });
            return activities;
        }

        private static List<Event> getEvents(ZenithContext db)
        {
            List<Event> events = new List<Event>();
            var e = new Event()
            {
                EventId = 1,
                FromDate = new DateTime(2016, 11, 5, 19, 0, 0),
                ToDate = new DateTime(2016, 11, 5, 20, 0, 0),
                CreatedBy = "Amanda",
                IsActive = true,
                CreationDate = new DateTime(2016, 11, 4, 12, 0, 0),
                Activity = db.Activity.First(a => a.ActivityId == 1)
            };
            events.Add(new Event()
            {
                EventId = 1,
                FromDate = new DateTime(2016, 11, 5, 19, 0, 0),
                ToDate = new DateTime(2016, 11, 5, 20, 0, 0),
                CreatedBy = "Amanda",
                IsActive = true,
                CreationDate = new DateTime(2016, 11, 4, 12, 0, 0),
                Activity = db.Activity.First(a => a.ActivityId == 1)
            });
            events.Add(new Event()
            {
                EventId = 2,
                FromDate = new DateTime(2016, 11, 6, 10, 30, 0),
                ToDate = new DateTime(2016, 11, 6, 12, 0, 0),
                CreatedBy = "Bob",
                IsActive = false,
                CreationDate = new DateTime(2016, 11, 5, 12, 0, 0),
                Activity = db.Activity.First(a => a.ActivityId == 2)
            });
            events.Add(new Event()
            {
                EventId = 3,
                FromDate = new DateTime(2016, 11, 7, 10, 30, 0),
                ToDate = new DateTime(2016, 11, 7, 12, 0, 0),
                CreatedBy = "Coot",
                IsActive = true,
                CreationDate = new DateTime(2016, 11, 6, 12, 0, 0),
                Activity = db.Activity.First(a => a.ActivityId == 3)
            });
            events.Add(new Event()
            {
                EventId = 4,
                FromDate = new DateTime(2016, 11, 7, 10, 30, 0),
                ToDate = new DateTime(2016, 11, 7, 12, 0, 0),
                CreatedBy = "Coot",
                IsActive = true,
                CreationDate = new DateTime(2016, 11, 6, 12, 0, 0),
                Activity = db.Activity.First(a => a.ActivityId == 1)
            });
            events.Add(new Event()
            {
                EventId = 5,
                FromDate = new DateTime(2016, 10, 28, 10, 30, 0),
                ToDate = new DateTime(2016, 10, 28, 12, 0, 0),
                CreatedBy = "Bob",
                IsActive = true,
                CreationDate = new DateTime(2016, 10, 20, 12, 0, 0),
                Activity = db.Activity.First(a => a.ActivityId == 2)
            });

            return events;
        }
    }
}
