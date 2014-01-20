using GameDataLayer;
using GameSketch;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Helpers;

namespace Web.Modules
{
    public class GameModule : NancyModule
    {

        private IRepository<Tournament> _repo;
        private TournamentService _service;

        public GameModule(IRepository<Tournament> repo, TournamentService service)
            : base(Href.ToNancyRouteAllInts(Href.TournamentsGames, "tournamentId"))
        {
            this._repo = repo;
            this._service = service;

            Get[Href.Root] = List;
            Get[Href.ToNancyRouteAllInts(Href.Get, "id")] = Display;
            Post[Href.Winner] = MarkWinner;
            Get[Href.Winner] = GetWinner;

            this.Before.AddItemToEndOfPipeline(new TournamentBeforeFilter(this._repo).Filter);
        }

        private dynamic List(dynamic arg)
        {
            Tournament tournament = arg.tournament;
            var gamesModel = new GamesModel();
            gamesModel.Games = tournament.Games.Select(game =>
                    new GameModel
                    {
                        Id = game.GameId,
                        PlayerIds = game.Players.Select(p => p.PlayerId).ToList(),
                        WinningPlayerId = game.Winner != null ? game.Winner.PlayerId : new Nullable<Int32>(),
                        Href = Href.TournamentsGameHref(tournament.TournamentId, game.GameId),
                        PlayersHref = Href.TournamentsGamePlayersHref(tournament.TournamentId, game.GameId),
                        WinningPlayerHref = game.Winner != null ? Href.TournamentsGameWinnerHref(tournament.TournamentId, game.GameId) : "",
                        MarkWinnerHref = Href.TournamentsGameMarkWinnerHref(tournament.TournamentId, game.GameId),
                        TournamentHref = Href.TournamentHref(tournament.TournamentId)
                    }).ToList();
            gamesModel.TournamentHref = Href.TournamentHref(tournament.TournamentId);
            return gamesModel;
        }

        private dynamic Display(dynamic arg)
        {
            throw new NotImplementedException();
        }

        private dynamic MarkWinner(dynamic arg)
        {
            throw new NotImplementedException();
        }

        private dynamic GetWinner(dynamic arg)
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