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

            this.Before.AddItemToEndOfPipeline(new TournamentBeforeFilter(this._repo).Filter);
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
                         new PlayerModel.Mapper(tournament).ToModel(p)
                    ).ToList()
            };
            playersModel.AddPlayerHref = Href.TournamentsPlayerCreateHref(tournament.TournamentId);
            playersModel.TournamentHref = Href.TournamentHref(tournament.TournamentId);
            return playersModel;
        }

        private dynamic Create(dynamic arg)
        {
            Tournament tournament = arg.tournament;
            var addPlayer = this.Bind<AddPlayerModel>();

            var newPlayer = new Player(addPlayer.Name);
            newPlayer = _service.AddPlayer(tournament, newPlayer);

            if (this.Request.Headers.Accept.Any(a => a.Item1.Contains("html")))
                return Response.AsRedirect(Href.TournamentsPlayerHref(tournament.TournamentId, newPlayer.PlayerId));

            return new PlayerModel.Mapper(tournament).ToModel(newPlayer);
        }

        private dynamic Display(dynamic arg)
        {
            Tournament tournament = arg.tournament;
            int playerId = arg.id;
            Player player = tournament.Players.SingleOrDefault(p => p.PlayerId == playerId);
            if (player == null) return new NotFoundResponse();

            return new PlayerModel.Mapper(tournament).ToModel(player);
        }
    }

    public class PlayersModel
    {
        public List<PlayerModel> Players { get; set; }
        public string TournamentHref { get; set; }
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
        public string TournamentPlayersHref { get; set; }

        public class Mapper
        {
            private Tournament tournament;
            public Mapper(Tournament tournament)
            {
                this.tournament = tournament;
            }

            public PlayerModel ToModel(Player player)
            {
                return new PlayerModel()
                {
                    Id = player.PlayerId,
                    Name = player.Name,
                    NumberOfWonGames = player.WonGames.Count(),
                    Href = Web.Href.TournamentsPlayerHref(tournament.TournamentId, player.PlayerId),
                    TournamentHref = Web.Href.TournamentHref(tournament.TournamentId),
                    TournamentPlayersHref = Web.Href.TournamentsPlayersHref(tournament.TournamentId),
                    GamesHref = Web.Href.TournamentsPlayerGamesHref(tournament.TournamentId, player.PlayerId)
                };
            }
        }
    }

    public class AddPlayerModel
    {
        public string Name { get; set; }
    }
}