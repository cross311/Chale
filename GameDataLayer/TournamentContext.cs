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

        private DbSet<Player> _Players { get; set; }

        private DbSet<Game> _Games { get; set; }

        private DbSet<Tournament> _Tournaments { get; set; }

        //public void Save()
        //{
        //    this.SaveChanges();
        //}

        //public IQueryable<Player> Players
        //{
        //    get { return _Players; }
        //}
        //public Player AddPlayer(Player player)
        //{
        //    return _Players.Add(player);
        //}


        //public IQueryable<Game> Games
        //{
        //    get { return _Games; }
        //}
        //public Game AddGame(Game game)
        //{
        //    return _Games.Add(game);
        //}


        //public IQueryable<Tournament> Tournaments
        //{
        //    get { return _Tournaments; }
        //}
        //public Tournament AddTournament(Tournament tournament)
        //{
        //    return _Tournaments.Add(tournament);
        //}
    }
}
