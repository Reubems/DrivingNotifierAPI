using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DrivingNotifierAPI.Models;

namespace DrivingNotifierAPI.Models
{
    public class DrivingNotifierAPIContext : DbContext
    {
        public DrivingNotifierAPIContext (DbContextOptions<DrivingNotifierAPIContext> options)
            : base(options)
        {
        }

        public DbSet<DrivingNotifierAPI.Models.User> User { get; set; }

        public DbSet<DrivingNotifierAPI.Models.Request> Request { get; set; }
    }
}
