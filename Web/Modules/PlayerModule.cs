using GameDataLayer;
using GameSketch;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Extensions;
using Nancy.ModelBinding;

namespace Web.Modules
{
    public class PlayerModule : NancyModule
    {
        
        private IRepository<Tournament> _repo;
        private TournamentService _service;

        public PlayerModule(IRepository<Tournament> repo, TournamentService service)
            : base("/tournaments/{tournamentId:int}/players/")
        {
            this._repo = repo;
            this._service = service;

            Get["/"] = List;
            Get["/create"] = CreateView;
            Post["/"] = Create;
            Get["/{id:int}"] = Display;

            this.Before.AddItemToEndOfPipeline(TournamentBeforeFilter);
        }

        private dynamic CreateView(dynamic arg)
        {
            Tournament tournament = arg.tournament;
            return View["Create", tournament.TournamentId];
        }

        private dynamic List(dynamic arg)
        {
            Tournament tournament = arg.tournament;
            PlayersModel playersModel = new PlayersModel()
            {
                Players = tournament.Players.Select(p =>
                        new PlayerModel
                        {
                            Id = p.PlayerId,
                            Uri = string.Format("{0}{1}/players/{2}", Context.ToFullPath("/tournaments/"), tournament.TournamentId, p.PlayerId),
                            Name = p.Name,
                            NumberOfWonGames = tournament.Games.Count(g => g.Winner != null && g.Winner == p)
                        }
                    ).ToList(),
                AddUri = string.Format("{0}{1}/players/create", Context.ToFullPath("/tournaments/"), tournament.TournamentId),
            };
            return playersModel;
        }

        private dynamic Create(dynamic arg)
        {
            Tournament tournament = arg.tournament;
            AddPlayerModel addPlayer = this.Bind<AddPlayerModel>();

            var newPlayer = new Player(addPlayer.Name);
            newPlayer = _service.AddPlayer(tournament, newPlayer);

            if (this.Request.Headers.Accept.Any(a => a.Item1.Contains("html")))
                return Response.AsRedirect(string.Format("/tournaments/{0}/players/{1}", tournament.TournamentId, newPlayer.PlayerId));

            return HttpStatusCode.NoContent;
        }

        private dynamic Display(dynamic arg)
        {
            throw new NotImplementedException();
        }

        private Nancy.Response TournamentBeforeFilter(NancyContext arg)
        {
            int tournamentId = arg.Parameters.tournamentId;
            var tournament = _repo.Get.SingleOrDefault(t => t.TournamentId == tournamentId);

            if (tournament == null) return new NotFoundResponse();

            arg.Parameters.tournament = tournament;
            return null;
        }
    }

    public class PlayersModel
    {
        public List<PlayerModel> Players { get; set; }

        public string AddUri { get; set; }
    }

    public class AddPlayerModel
    {
        public string Name { get; set; }
    }
}