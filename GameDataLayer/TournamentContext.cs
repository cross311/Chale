using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace GameDataLayer
{
    public class TournamentContext : DbContext
    {
        public TournamentContext() : base("DefaultConnection"){}

        public DbSet<Player> Players { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Tournament> Tournaments { get; set; }
    }
}
