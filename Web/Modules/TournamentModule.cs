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
            Get["/create"] = _ => View["Create"];
            Post["/"] = Create;
            Get["/{id:int}"] = Display;
            Post["/{id:int}/start"] = Start;
        }

        private dynamic Display(dynamic arg)
        {
            Int32 id = arg.id;
            var tournament = _repo.Get.SingleOrDefault(t => t.TournamentId == id);
            return FillInActions(CreateTournamentModel(tournament));
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
                    Players = tournament.Players.Select(p =>
                        new PlayerModel
                        {
                            Id = p.PlayerId,
                            Name = p.Name,
                            NumberOfWonGames = tournament.Games.Count(g => g.Winner != null && g.Winner == p)
                        }
                    ).ToList(),
                    NumberOfGames = tournament.Games.Count,
                    Games = tournament.Games.Select(g =>
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
            viewmodel.Actions.Add(new ActionModel("get", Context.ToFullPath("/tournaments/create"), "Add Tournament"));
            viewmodel.Tournaments.ForEach(t => FillInActions(t));

            return viewmodel;
        }

        private TournamentModel FillInActions(TournamentModel tournamentModel)
        {
            tournamentModel.Href = Context.ToFullPath(string.Format("~/tournaments/{0}", tournamentModel.Id));

            tournamentModel.Actions.Add(new ActionModel("get", string.Format("{0}/players", tournamentModel.Href), "View Players"));
            if (!tournamentModel.IsStarted && tournamentModel.NumberOfPlayers > 0)
                tournamentModel.Actions.Add(new ActionModel("post", string.Format("{0}/start", tournamentModel.Href), "Start Tournament"));
            return tournamentModel;
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
                Players = tournament.Players.Select(p =>
                    new PlayerModel
                    {
                        Id = p.PlayerId,
                        Name = p.Name,
                        NumberOfWonGames = tournament.Games.Count(g => g.Winner != null && g.Winner == p)
                    }
                ).ToList(),
                NumberOfGames = tournament.Games.Count,
                Games = tournament.Games.Select(g =>
                    new GameModel
                    {
                        Id = g.GameId,
                        PlayerIds = g.Players.Select(p =>
                            p.PlayerId).ToList(),
                        WinningPlayerId = g.Winner != null ? g.Winner.PlayerId : -1
                    }
                ).ToList()
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
        public TournamentsModel()
        {
            Actions = new List<ActionModel>();
        }
        public List<TournamentModel> Tournaments { get; set; }
        public List<ActionModel> Actions { get; set; }
    }

    public class TournamentModel
    {
        public TournamentModel()
        {
            Players = new List<PlayerModel>();
            Games = new List<GameModel>();
            Actions = new List<ActionModel>();
        }

        public Int32 Id { get; set; }
        public string Href { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsStarted { get; set; }
        public int NumberOfPlayers { get; set; }
        public int NumberOfGames { get; set; }
        public List<PlayerModel> Players { get; set; }
        public List<GameModel> Games { get; set; }
        public List<ActionModel> Actions { get; set; }
    }

    public class PlayerModel
    {
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public int NumberOfWonGames { get; set; }
        public string Href { get; set; }
    }

    public class GameModel
    {
        public Int32 Id { get; set; }
        public List<Int32> PlayerIds { get; set; }
        public Int32 WinningPlayerId { get; set; }
    }

    public class ActionModel
    {
        public ActionModel(string method, string href, string prompt)
        {
            this.Method = method;
            this.Href = href;
            this.Prompt = prompt;
        }
        public string Href { get; set; }
        public string Method { get; set; }
        public string Prompt { get; set; }
    }
}