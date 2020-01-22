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

        public DbSet<Person> Person { get; set; }
    }
}