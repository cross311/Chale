using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSketch
{
    public interface ITournamentFactory
    {
        Tournament BuildNewTournament(string name, DateTime startDate, IList<Dude> dudes);
    }

    public class TournamentFactory : ITournamentFactory
    {
        private readonly IRepository<Tournament> _Repository;

        public TournamentFactory(IRepository<Tournament> repository)
        {
            _Repository = repository;
        }

        public Tournament BuildNewTournament(string name, DateTime startDate)
        {
            throw new NotImplementedException();

            var result = BuildNewTournament(name, startDate, new List<Dude>());
            return result;
        }

        public Tournament BuildNewTournament(string name, DateTime startDate, IList<Dude> dudes)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name is requried", "name");
            }

            var tournament = new Tournament(name, startDate, dudes, new List<Game>(), null, 0);

            var result = _Repository.Save(tournament);
            return result;
        }


    }
}
