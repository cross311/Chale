using GameDataLayer;
using GameSketch;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Extensions;
using Nancy.ModelBinding;
using Web.Helpers;

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
            Post[Href.Create] = Create;
            Get[Href.ToNancyRouteAllInts(Href.Get, "id")] = Display;
            Post[Href.ToNancyRouteAllInts(Href.Start, "id")] = Start;
        }

        private dynamic Display(dynamic arg)
        {
            var tournament = TournamentFromUrl(arg);
            if (tournament == null) return new NotFoundResponse();
            return ToTournamentModel(tournament);
        }

        private Tournament TournamentFromUrl(dynamic arg)
        {
            Int32 id = arg.id;
            var tournament = _repo.Get.SingleOrDefault(t => t.TournamentId == id);
            return tournament;
        }

        private static TournamentModel ToTournamentModel(Tournament tournament)
        {
            return new TournamentModel
            {
                Id              = tournament.TournamentId,
                Name            = tournament.Name,
                Description     = tournament.Description,
                IsStarted       = tournament.Games.Any(),
                NumberOfPlayers = tournament.Players.Count,
                NumberOfGames   = tournament.Games.Count,
                Href            = Href.TournamentHref(tournament.TournamentId),
                PlayersHref     = Href.TournamentsPlayersHref(tournament.TournamentId),
                GamesHref       = Href.TournamentsGamesHref(tournament.TournamentId),
                TournamentsHref = Href.Tournaments
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
                    Id              = tournament.TournamentId,
                    Name            = tournament.Name,
                    Description     = tournament.Description,
                    IsStarted       = tournament.Games.Any(),
                    NumberOfPlayers = tournament.Players.Count,
                    NumberOfGames   = tournament.Games.Count,
                }).ToList()
            };

            viewmodel.Tournaments.ForEach(t =>
            {
                t.Href        = Href.TournamentHref(t.Id);
                t.PlayersHref = Href.TournamentsPlayersHref(t.Id);
                t.GamesHref   = Href.TournamentsGamesHref(t.Id);
            });
            viewmodel.CreateTournamentHref = Href.TournamentCreate;

            return viewmodel;
        }

        private dynamic Create(dynamic _)
        {
            var createViewModel = this.Bind<TournamentCreateModel>();

            var newTournament = _service.Create(createViewModel.Name, createViewModel.Description);

            var newTournamentLocation = Href.TournamentHref(newTournament.TournamentId);

            if (this.Request.Headers.Accept.Any(a => a.Item1.Contains("html")))
                return Response.AsRedirect("~" + newTournamentLocation);

            return ToTournamentModel(newTournament);
        }

        private dynamic Start(dynamic arg)
        {
            int tournamentId = arg.id;
            var tournament = _repo.Get.SingleOrDefault(t => t.TournamentId == tournamentId);
            if (tournament == null) return new NotFoundResponse();

            tournament = _service.Start(tournament);

            if (this.Request.Headers.Accept.Any(a => a.Item1.Contains("html")))
                return Response.AsRedirect("~" + Href.TournamentHref(tournament.TournamentId));

            return HttpStatusCode.Accepted;
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

        public string TournamentsHref { get; set; }
    }
}