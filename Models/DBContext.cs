using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IMDeanyP.Models
{
    public class DBContext : DbContext
    {
        public DbSet<Film> Films { get; set; }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Acting> Actings { get; set; }

        public DbSet<Review> Reviews { get; set; }

    }
}