using GameDataLayer;
using GameSketch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Extensions;

namespace Web.Modules
{
    public class TournamentModule : NancyModule
    {
        private IRepository<Tournament> _repo;
        private TournamentService _service;

        public TournamentModule(IRepository<Tournament> repo, TournamentService service)
            : base(Href.Tournaments)
        {
            this._repo = repo;
            this._service = service;

            Get[Href.Root] = List;
            Get[Href.Create] = _ => View["Create"];
            Post[Href.Root] = Create;
            Get[Href.ToNancyRouteAllInts(Href.Get, "id")] = Display;
            Post[Href.ToNancyRouteAllInts(Href.Start, "id")] = Start;
        }

        private dynamic Display(dynamic arg)
        {
            Int32 id = arg.id;
            var tournament = _repo.Get.SingleOrDefault(t => t.TournamentId == id);
            return new TournamentModel
            {
                Id = tournament.TournamentId,
                Name = tournament.Name,
                Description = tournament.Description,
                IsStarted = tournament.Games.Any(),
                NumberOfPlayers = tournament.Players.Count,
                NumberOfGames = tournament.Games.Count,
                Href = Href.TournamentHref(tournament.TournamentId),
                PlayersHref = Href.TournamentsPlayersHref(tournament.TournamentId),
                GamesHref = Href.TournamentsGamesHref(tournament.TournamentId)
            };
        }

        private dynamic List(dynamic _)
        { return List(); }

        private dynamic List()
        {
            var viewmodel = new TournamentsModel()
            {
                Tournaments = _repo.Get.Select(tournament => new TournamentModel
                {
                    Id = tournament.TournamentId,
                    Name = tournament.Name,
                    Description = tournament.Description,
                    IsStarted = tournament.Games.Any(),
                    NumberOfPlayers = tournament.Players.Count,
                    NumberOfGames = tournament.Games.Count,
                }).ToList()
            };

            viewmodel.Tournaments.ForEach(t =>
            {
                t.Href = Href.TournamentHref(t.Id);
                t.PlayersHref = Href.TournamentsPlayersHref(t.Id);
                t.GamesHref = Href.TournamentsGamesHref(t.Id);
            });
            viewmodel.CreateTournamentHref = Href.TournamentCreate;

            return viewmodel;
        }

        private TournamentModel CreateTournamentModel(Tournament tournament)
        {
            return new TournamentModel
            {
                Id = tournament.TournamentId,
                Name = tournament.Name,
                Description = tournament.Description,
                IsStarted = tournament.IsStarted(),
                NumberOfPlayers = tournament.Players.Count,
                NumberOfGames = tournament.Games.Count,
            };
        }

        private dynamic Create(dynamic _)
        {
            var createViewModel = this.Bind<TournamentCreateModel>();

            var newTournament = _service.Create(createViewModel.Name, createViewModel.Description);

            if (this.Request.Headers.Accept.Any(a => a.Item1.Contains("html")))
                return Response.AsRedirect("~/tournaments/");

            return HttpStatusCode.NoContent;
        }

        private dynamic Start(dynamic arg)
        {
            int tournamentId = arg.id;
            var tournament = _repo.Get.SingleOrDefault(t => t.TournamentId == tournamentId);
            if (tournament == null) return new NotFoundResponse();

            tournament = _service.Start(tournament);

            if (this.Request.Headers.Accept.Any(a => a.Item1.Contains("html")))
                return Response.AsRedirect(string.Format("~/tournaments/{0}", tournament.TournamentId));

            return HttpStatusCode.NoContent;
        }
    }

    public class TournamentCreateModel
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class TournamentsModel
    {
        public List<TournamentModel> Tournaments { get; set; }
        public string CreateTournamentHref { get; set; }
    }

    public class TournamentModel
    {
        public Int32 Id { get; set; }
        public string Href { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsStarted { get; set; }
        public int NumberOfPlayers { get; set; }
        public int NumberOfGames { get; set; }
        public string PlayersHref { get; set; }
        public string GamesHref { get; set; }
        public string AddPlayerHref { get; set; }
    }

    public class PlayerModel
    {
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public int NumberOfWonGames { get; set; }
        public string Href { get; set; }
        public string GamesHref { get; set; }
        public string TournamentHref { get; set; }
    }

    public class GameModel
    {
        public Int32 Id { get; set; }
        public List<Int32> PlayerIds { get; set; }
        public Int32 WinningPlayerId { get; set; }
        public string PlayersHref { get; set; }
        public string WinningPlayerHref { get; set; }
        public string TournamentHref { get; set; }
        public string MarkWinnerHref { get; set; }
    }
}