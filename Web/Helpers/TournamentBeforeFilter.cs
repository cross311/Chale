using GameDataLayer;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Helpers
{
    public class TournamentBeforeFilter
    {
        private IRepository<Tournament> _repo;

        public TournamentBeforeFilter(IRepository<Tournament> repo)
        {
            _repo = repo;
        }


        public Nancy.Response Filter(NancyContext arg)
        {
            int tournamentId = arg.Parameters.tournamentId;
            var tournament = _repo.Get.SingleOrDefault(t => t.TournamentId == tournamentId);

            if (tournament == null) return new NotFoundResponse();

            arg.Parameters.tournament = tournament;
            return null;
        }
    }
}