using System.Collections.Generic;
using BotMedicUa.Models;
using BotMedicUa.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace BotMedicUa.Context
{
    public class DataContext:DbContext
    {
        public DbSet<Patient> Patient { get; set; }
        public DbSet<Record> Record { get; set; }
        public DbSet<Doctor> Doctor { get; set; }
        public DbSet<Schedule> Schedule { get; set; }

        public DataContext()
        {
            Database.EnsureCreated();
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Hospital-0e9;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}