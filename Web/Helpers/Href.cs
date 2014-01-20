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

        private const string StartAction = "/start";

        public const string Start = Get + StartAction;

        private const string WinnerAction = "/winner";

        public const string Winner = Get + WinnerAction;

        public const string Tournament = Tournaments + Get;

        public const string TournamentCreate = Tournaments + Create;

        public const string TournamentWinner = Tournament + WinnerAction;

        public const string TournamentStart = Tournaments + Start;

        public const string Player = Players + Get;

        public const string PlayerCreate = Players + Create;

        public const string Game = Games + Get;

        public const string GameWinner = Game + WinnerAction;

        public const string TournamentsPlayers = Tournament + Players;

        public const string TournamentsPlayer = TournamentsPlayers + GetChild;

        public const string TournamentsPlayerCreate = TournamentsPlayers + Create;

        public const string TournamentsPlayerGames = TournamentsPlayer + Games;

        public const string TournamentsGames = Tournament + Games;

        public const string TournamentsGame = TournamentsGames + GetChild;

        public const string TournamentsGamePlayers = TournamentsGame + Players;

        public const string TournamentsGameWinner = TournamentsGame + WinnerAction;

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

        public static string TournamentsPlayerGamesHref(Int32 tournamentId, Int32 playerId)
        {
            return TournamentsPlayerGamesHref(tournamentId.ToString(), playerId.ToString());
        }

        public static string TournamentsPlayerGamesHref(string tournamentId, string playerId)
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

        public static string TournamentsGamePlayersHref(Int32 tournamentId, Int32 gameId)
        {
            return TournamentsGamePlayersHref(tournamentId.ToString(), gameId.ToString());
        }

        public static string TournamentsGamePlayersHref(string tournamentId, string gameId)
        {
            return string.Format(TournamentsGamePlayers, tournamentId, gameId);
        }

        public static string TournamentsGameWinnerHref(Int32 tournamentId, Int32 gameId)
        {
            return TournamentsGameWinnerHref(tournamentId.ToString(), gameId.ToString());
        }

        public static string TournamentsGameWinnerHref(string tournamentId, string gameId)
        {
            return string.Format(TournamentsGameWinner, tournamentId, gameId);
        }

        public static string TournamentsGamePostWinnerHref(Int32 tournamentId, Int32 gameId)
        {
            return TournamentsGamePostWinnerHref(tournamentId.ToString(), gameId.ToString());
        }

        public static string TournamentsGamePostWinnerHref(string tournamentId, string gameId)
        {
            return string.Format(TournamentsGameWinner, tournamentId, gameId);
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