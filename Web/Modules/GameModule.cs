using GameDataLayer;
using GameSketch;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Modules
{
    public class GameModule : NancyModule
    {
        
        private IRepository<Tournament> _repo;
        private TournamentService _service;

        public GameModule(IRepository<Tournament> repo, TournamentService service)
            : base(Href.ToNancyRouteAllInts(Href.TournamentsGames,"tournamentId"))
        {
            this._repo = repo;
            this._service = service;

            Get[Href.Root] = List;
            Get[Href.ToNancyRouteAllInts(Href.Get, "id")] = Display;
            Post[Href.Winner] = Winner;
        }

        private dynamic List(dynamic arg)
        {
            throw new NotImplementedException();
        }

        private dynamic Display(dynamic arg)
        {
            throw new NotImplementedException();
        }

        private dynamic Winner(dynamic arg)
        {
            throw new NotImplementedException();
        }

        public class GamesModel
        {
            public List<GameModel> Games { get; set; }
            public string TournamentHref { get; set; }
        }

        public class MarkWinnerModel
        {
            public Int32 WinningPlayerId { get; set; }
        }
    }
}