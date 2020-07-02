using Microsoft.EntityFrameworkCore;
using ReversiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiApp.DAL
{
    public class SpelContext : DbContext
    {
        public SpelContext(DbContextOptions<SpelContext> options) : base(options) { }

        public DbSet<Spel> Spel { get; set; }

    }
}
