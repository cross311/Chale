using GameDataLayer;
using GameSketch;
using Nancy;
using Nancy.ModelBinding;
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
            Post[Href.ToNancyRouteAllInts(Href.Winner, "id")] = PostWinner;

            this.Before.AddItemToEndOfPipeline(new TournamentBeforeFilter(this._repo).Filter);
        }

        private dynamic List(dynamic arg)
        {
            Tournament tournament = arg.tournament;
            var gamesModel = new GamesModel();
            gamesModel.Games = tournament.Games.Select(new GameModel.Mapper(tournament).ToModel).ToList();
            gamesModel.TournamentHref = Href.TournamentHref(tournament.TournamentId);
            return gamesModel;
        }

        private dynamic Display(dynamic arg)
        {
            Tournament tournament = arg.tournament;
            Int32 gameId = arg.id;
            Game game = tournament.Games.SingleOrDefault(g => g.GameId == gameId);
            if (game == null) return new NotFoundResponse();

            return new GameModel.Mapper(tournament).ToModel(game);
        }

        private dynamic PostWinner(dynamic arg)
        {
            Tournament tournament = arg.tournament;
            Int32 gameId = arg.id;
            PostWinnerModel postWinnerModel = this.Bind<PostWinnerModel>();

            Game game = tournament.Games.SingleOrDefault(g => g.GameId == gameId);
            if (game == null) return new NotFoundResponse();

            var winningPlayer = game.Players.SingleOrDefault(p => p.PlayerId == postWinnerModel.WinningPlayerId);
            if (winningPlayer == null) return new NotFoundResponse();

            tournament = _service.GameWon(tournament, game, winningPlayer);

            if (this.Request.Headers.Accept.Any(a => a.Item1.Contains("html")))
                return Response.AsRedirect(Href.TournamentsGameHref(tournament.TournamentId, game.GameId));

            return HttpStatusCode.NoContent;

        }

        public class GamesModel
        {
            public List<GameModel> Games { get; set; }
            public string TournamentHref { get; set; }
        }

        public class GameModel
        {
            public Int32 Id { get; set; }
            public List<PlayerModel> Players { get; set; }
            public PlayerModel WinningPlayer { get; set; }
            public string TournamentHref { get; set; }
            public string PostWinnerHref { get; set; }
            public string Href { get; set; }
            public string TournamentGamesHref { get; set; }

            public class Mapper
            {
                private Tournament tournament;
                public Mapper(Tournament tournament)
                {
                    this.tournament = tournament;
                }

                public GameModel ToModel(Game game)
                {
                    return new GameModel
                    {
                        Id = game.GameId,
                        Href = Web.Href.TournamentsGameHref(tournament.TournamentId, game.GameId),
                        PostWinnerHref = Web.Href.TournamentsGamePostWinnerHref(tournament.TournamentId, game.GameId),
                        TournamentHref = Web.Href.TournamentHref(tournament.TournamentId),
                        TournamentGamesHref = Web.Href.TournamentsGamesHref(tournament.TournamentId),
                        Players = game.Players.Select(new PlayerModel.Mapper(tournament).ToModel).ToList(),
                        WinningPlayer = game.Winner != null ? new PlayerModel.Mapper(tournament).ToModel(game.Winner) : default(PlayerModel)
                    };
                }
            }
        }

        public class PostWinnerModel
        {
            public Int32 WinningPlayerId { get; set; }
        }
    }
}