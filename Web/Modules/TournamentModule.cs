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
            : base("/tournaments")
        {
            this._repo = repo;
            this._service = service;

            Get["/"] = List;
            Get["/Create"] = _ => View["Create"];
            Post["/"] = Create;
        }

        private dynamic List(dynamic _)
        { return List(); }

        private dynamic List()
        {
            var viewmodel = new TournamentsModel()
            {
                CreateUri = Context.ToFullPath("/tournaments/create"),
                Tournaments = _repo.Get.Select(t => new TournamentModel
                    {
                        Id = t.TournamentId,
                        Name = t.Name,
                        Description = t.Description,
                        NumberOfPlayers = t.Players.Count,
                        Players = t.Players.Select(p =>
                            new PlayerModel
                            {
                                Id = p.PlayerId,
                                Name = p.Name,
                                NumberOfWonGames = t.Games.Count(g => g.Winner != null && g.Winner == p)
                            }
                        ).ToList(),
                        NumberOfGames = t.Games.Count,
                        Games = t.Games.Select(g =>
                            new GameModel
                            {
                                Id = g.GameId,
                                PlayerIds = g.Players.Select(p =>
                                    p.PlayerId).ToList(),
                                WinningPlayerId = g.Winner != null ? g.Winner.PlayerId : -1
                            }
                        ).ToList()
                    }).ToList()
            };
            var tournamentPath = Context.ToFullPath("/tournaments/");
            viewmodel.Tournaments.ForEach(t => t.Uri = string.Format("{0}{1}", tournamentPath, t.Id));

            return viewmodel;
        }

        private dynamic Create(dynamic _)
        {
            var createViewModel = this.Bind<TournamentCreateModel>();

            var newTournament = _service.Create(createViewModel.Name, createViewModel.Description);

            if (this.Request.Headers.Accept.Any(a => a.Item1.Contains("html")))
                return Response.AsRedirect("/tournaments/");

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
        public string CreateUri { get; set; }
        public List<TournamentModel> Tournaments { get; set; }
    }

    public class TournamentModel
    {
        public Int32 Id { get; set; }
        public string Uri { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfPlayers { get; set; }
        public int NumberOfGames { get; set; }
        public List<PlayerModel> Players { get; set; }
        public List<GameModel> Games { get; set; }
    }

    public class PlayerModel
    {
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public int NumberOfWonGames { get; set; }
    }

    public class GameModel
    {
        public Int32 Id { get; set; }
        public List<Int32> PlayerIds { get; set; }
        public Int32 WinningPlayerId { get; set; }
    }
}