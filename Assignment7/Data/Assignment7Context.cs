using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Assignment7.Models;

namespace Assignment7.Data
{
    public class Assignment7Context : DbContext
    {
        public Assignment7Context (DbContextOptions<Assignment7Context> options)
            : base(options)
        {
        }

        public DbSet<Assignment7.Models.Student> Student { get; set; } = default!;
        public DbSet<Assignment7.Models.Course> Course { get; set; } = default!;
    }
}
