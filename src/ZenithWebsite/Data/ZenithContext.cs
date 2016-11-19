using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZenithWebsite.Models.ZenithSocietyModels;

namespace ZenithWebsite.Data
{
    public class ZenithContext : DbContext
    {
        public ZenithContext(DbContextOptions<ZenithContext> options) : base(options) { }
        public DbSet<Activity> Activity { get; set; }
        public DbSet<Event> Event { get; set; }
    }
}
