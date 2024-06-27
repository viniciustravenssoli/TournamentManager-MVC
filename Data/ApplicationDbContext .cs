using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TorneioManager.Models;

namespace TorneioManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Match> Matches { get; set; }
    }
}