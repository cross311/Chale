using GameDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSketch
{
    //public interface ITournamentFactory
    //{
    //    Tournament BuildNewTournament(string name, string description, DateTime startDate, IList<Player> dudes);
    //}

    //public class TournamentFactory : ITournamentFactory
    //{
    //    private readonly IRepository<Tournament> _Repository;

    //    public TournamentFactory(IRepository<Tournament> repository)
    //    {
    //        _Repository = repository;
    //    }

    //    public Tournament BuildNewTournament(string name, string description, DateTime startDate, IList<Player> players)
    //    {
    //        if (String.IsNullOrWhiteSpace(name))
    //        {
    //            throw new ArgumentException("name is requried", "name");
    //        }

    //        var tournament = new Tournament(name, description, startDate, players, new List<Game>());

    //        var result = _Repository.Save(tournament);
    //        return result;
    //    }


    //}
}