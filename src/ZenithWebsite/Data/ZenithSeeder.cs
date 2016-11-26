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
            Random rand = new Random();
            string[] randomNames = { "Amanda", "Bob", "Charles", "Dick", "Elaine", "Ford", "Zenith" };

            // Base values 
            var startDate = new DateTime(2016, 11, 1, 0, 0, 0);
            var idCounter = 1;
            var dayCounter = 1;

            // Return value 
            List<Event> events = new List<Event>();

            for (int i = 0; i < 50; i++)
            {
                idCounter += 1;
                dayCounter += rand.Next(4); // 0-3

                var newStartDate = startDate.AddDays(dayCounter);
                newStartDate = newStartDate.AddHours(rand.Next(23));
                newStartDate = newStartDate.AddMinutes(rand.Next(60));

                var newEndDate = new DateTime(
                    newStartDate.Year, 
                    newStartDate.Month, 
                    newStartDate.Day, 
                    newStartDate.Hour + 1, 
                    newStartDate.Minute,
                    newStartDate.Second
                );

                var randomName = randomNames[rand.Next(randomNames.Length)];

                var randomActive = rand.Next(2) == 1 ? true : false;

                var randomActivityId = rand.Next(1, 4);

                var newEvent = new Event()
                {
                    EventId = idCounter,
                    FromDate = newStartDate,
                    ToDate = newEndDate,
                    CreatedBy = randomName,
                    IsActive = randomActive,
                    CreationDate = newStartDate,
                    Activity = db.Activity.First(a => a.ActivityId == randomActivityId)
                };

                events.Add(newEvent);
            }

            return events;
        }
    }
}
