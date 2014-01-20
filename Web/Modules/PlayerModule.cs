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
            : base(Href.ToNancyRouteAllInts(Href.TournamentsPlayers, "tournamentId"))
        {
            this._repo = repo;
            this._service = service;

            Get[Href.Root] = List;
            Get[Href.Create] = CreateView;
            Post[Href.Root] = Create;
            Get[Href.ToNancyRouteAllInts(Href.Get, "id")] = Display;

            this.Before.AddItemToEndOfPipeline(TournamentBeforeFilter);
        }

        private dynamic CreateView(dynamic arg)
        {
            Tournament tournament = arg.tournament;
            return View["Create", Href.TournamentsPlayersHref(tournament.TournamentId)];
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
                            Href = Href.TournamentsPlayerHref(tournament.TournamentId, p.PlayerId),
                            GamesHref = Href.TournamentsPlayerGamesHref(tournament.TournamentId, p.PlayerId),
                            TournamentHref = Href.TournamentHref(tournament.TournamentId),
                            Name = p.Name,
                            NumberOfWonGames = tournament.Games.Count(g => g.Winner != null && g.Winner == p)
                        }
                    ).ToList()
            };
            playersModel.AddPlayerHref = Href.TournamentsPlayerCreateHref(tournament.TournamentId);
            playersModel.TournamentHref = Href.TournamentHref(tournament.TournamentId);
            return playersModel;
        }

        private dynamic Create(dynamic arg)
        {
            Tournament tournament = arg.tournament;
            AddPlayerModel addPlayer = this.Bind<AddPlayerModel>();

            var newPlayer = new Player(addPlayer.Name);
            newPlayer = _service.AddPlayer(tournament, newPlayer);

            if (this.Request.Headers.Accept.Any(a => a.Item1.Contains("html")))
                return Response.AsRedirect(Href.TournamentsPlayerHref(tournament.TournamentId, newPlayer.PlayerId));

            return HttpStatusCode.NoContent;
        }

        private dynamic Display(dynamic arg)
        {
            Tournament tournament = arg.tournament;
            int playerId = arg.id;
            Player player = tournament.Players.SingleOrDefault(p => p.PlayerId == playerId);
            if (player == null) return new NotFoundResponse();

            return new PlayerModel()
            {
                Id = player.PlayerId,
                Name = player.Name,
                NumberOfWonGames = player.WonGames.Count(),
                Href = Href.TournamentsPlayerHref(tournament.TournamentId, player.PlayerId),
                TournamentHref = Href.TournamentHref(tournament.TournamentId),
                TournamentPlayersHref = Href.TournamentsPlayersHref(tournament.TournamentId),
                GamesHref = Href.TournamentsPlayerGamesHref(tournament.TournamentId, player.PlayerId)
            };
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
        public string TournamentHref { get; set; }
        public string AddPlayerHref { get; set; }
    }

    public class AddPlayerModel
    {
        public string Name { get; set; }
    }
}