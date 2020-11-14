using Microsoft.EntityFrameworkCore;
using ScholarStatistics.DAL.Models;
using System;

namespace ScholarStatistics.DAL
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Publication> Publications { get; set; }
        public DbSet<Affiliation> Affiliations { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

    }
}
