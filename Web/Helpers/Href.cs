using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web
{
    public static class Href
    {
        public const string Root = "/";

        public const string Tournaments = "/tournaments";

        public const string Players = "/players";

        public const string Games = "/games";

        public const string Create = "/create";

        public const string Get = "/{0}";

        public const string GetChild = "/{1}";

        public const string Start = Get + "/start";

        public const string Winner = Get + "/winner";

        public const string MarkWinner = Get + "/markwinner";

        public const string Tournament = Tournaments + Get;

        public const string TournamentCreate = Tournaments + Create;

        public const string TournamentWinner = Tournaments + Winner;

        public const string TournamentStart = Tournaments + Start;

        public const string Player = Players + Get;

        public const string PlayerCreate = Players + Create;

        public const string Game = Games + Get;

        public const string GameWinner = Games + Winner;

        public const string GameMarkWinner = Games + MarkWinner;

        public const string TournamentsPlayers = Tournament + Players;

        public const string TournamentsPlayer = TournamentsPlayers + GetChild;

        public const string TournamentsPlayerCreate = TournamentsPlayers + Create;

        public const string TournamentsPlayerGames = TournamentsPlayer + Games;

        public const string TournamentsGames = Tournament + Games;

        public const string TournamentsGame = TournamentsGames + GetChild;

        public const string TournamentsGamePlayers = TournamentsGame + Players;

        public const string TournamentsGameWinner = TournamentsGame + Winner;

        public const string TournamentsGameMarkWinner = TournamentsGame + MarkWinner;

        public static string TournamentHref(Int32 tournamentId)
        {
            return TournamentHref(tournamentId.ToString());
        }

        public static string TournamentHref(string tournamentId)
        {
            return string.Format(Tournament, tournamentId);
        }

        public static string TournamentsPlayersHref(Int32 tournamentId)
        {
            return TournamentsPlayersHref(tournamentId.ToString());
        }

        public static string TournamentsPlayersHref(string tournamentId)
        {
            return string.Format(TournamentsPlayers, tournamentId);
        }

        public static string TournamentsPlayerHref(Int32 tournamentId, Int32 playerId)
        {
            return TournamentsPlayerHref(tournamentId.ToString(), playerId.ToString());
        }

        public static string TournamentsPlayerHref(string tournamentId, string playerId)
        {
            return string.Format(TournamentsPlayer, tournamentId, playerId);
        }

        public static string TournamentsPlayerCreateHref(Int32 tournamentId)
        {
            return TournamentsPlayerCreateHref(tournamentId.ToString());
        }

        public static string TournamentsPlayerCreateHref(string tournamentId)
        {
            return string.Format(TournamentsPlayerCreate, tournamentId);
        }

        public static string TournamentsPlayersGamesHref(Int32 tournamentId, Int32 playerId)
        {
            return TournamentsPlayersGamesHref(tournamentId.ToString(), playerId.ToString());
        }

        public static string TournamentsPlayersGamesHref(string tournamentId, string playerId)
        {
            return string.Format(TournamentsPlayerGames, tournamentId, playerId);
        }

        public static string TournamentsGamesHref(Int32 tournamentId)
        {
            return TournamentsGamesHref(tournamentId.ToString());
        }

        public static string TournamentsGamesHref(string tournamentId)
        {
            return string.Format(TournamentsGames, tournamentId);
        }

        public static string TournamentsGameHref(Int32 tournamentId, Int32 gameId)
        {
            return TournamentsGameHref(tournamentId.ToString(), gameId.ToString());
        }

        public static string TournamentsGameHref(string tournamentId, string gameId)
        {
            return string.Format(TournamentsGame, tournamentId, gameId);
        }

        public static string TournamentsGamesPlayersHref(Int32 tournamentId, Int32 gameId)
        {
            return TournamentsGamesPlayersHref(tournamentId.ToString(), gameId.ToString());
        }

        public static string TournamentsGamesPlayersHref(string tournamentId, string gameId)
        {
            return string.Format(TournamentsGamePlayers, tournamentId, gameId);
        }

        public static string TournamentsGamesWinnerHref(Int32 tournamentId, Int32 gameId)
        {
            return TournamentsGamesWinnerHref(tournamentId.ToString(), gameId.ToString());
        }

        public static string TournamentsGamesWinnerHref(string tournamentId, string gameId)
        {
            return string.Format(TournamentsGameWinner, tournamentId, gameId);
        }

        public static string TournamentsGameMarkWinnerHref(Int32 tournamentId, Int32 gameId)
        {
            return TournamentsGameMarkWinnerHref(tournamentId.ToString(), gameId.ToString());
        }

        public static string TournamentsGameMarkWinnerHref(string tournamentId, string gameId)
        {
            return string.Format(TournamentsGameMarkWinner, tournamentId, gameId);
        }

        public static string ToNancyRoute(string href, params string[] routeReplacements)
        {
            return string.Format(href, routeReplacements);
        }

        public static string ToNancyRouteAllInts(string href, params string[] routeParams)
        {
            string[] intRouteParams = new string[routeParams.Length];
            for (int i = 0; i < routeParams.Length; i++)
			{
		        intRouteParams[i] = string.Format("{0}{1}{2}","{", routeParams[i],":int}");
	        }
            return ToNancyRoute(href, intRouteParams);
        }
    }
}