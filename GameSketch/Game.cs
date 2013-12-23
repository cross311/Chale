using GameDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSketch
{

    public class TournamentService
    {
        const int NumberOfPlayersPerGame = 2;
        readonly IRepository<Tournament> _tournamentRepo;

        public TournamentService(IRepository<Tournament> tournamentRepo)
        {
            this._tournamentRepo = tournamentRepo;
        }

        public Tournament Start(Tournament tournament)
        {
            var players = tournament.Players;
            var numberOfGames = NumberOfGamesForNumberOfDudes(NumberOfPlayersPerGame, players.Count);
            for (int gameNumber = 0; gameNumber < numberOfGames; gameNumber++)
            {
                tournament.AddGame(new Game(1, players.Skip(gameNumber * NumberOfPlayersPerGame).Take(NumberOfPlayersPerGame)));
            }

            var result = _tournamentRepo.Save(tournament);
            return result;
        }

        public Tournament GameWon(Tournament tournament, Game wonGame, Player playerWhoWon)
        {
            wonGame.Winner = playerWhoWon;
            var openGame = tournament.Games.FirstOrDefault(game => game.IsOpen());
            if (openGame != null)
                openGame.AddPlayer(playerWhoWon);
            else if (tournament.Games.Any(game => game.IsInProgress()))
                tournament.AddGame(new Game(wonGame.Level + 1, new Player[] { playerWhoWon }));
            else if (tournament.Games.All(game => game.IsCompleted()))
                tournament.Winner = playerWhoWon;

            return GameWonLogic(tournament, wonGame, playerWhoWon);
        }

        private Tournament GameWonLogic(Tournament tournament, Game wonGame, Player playerWhoWon)
        {
            if(!FilledInOpenGame(tournament, playerWhoWon))
                if(!PutTournamentOnHoldForLaggingGame(tournament, wonGame, playerWhoWon))
                    if(!CreatedOpenGame(tournament, wonGame, playerWhoWon))
                        TournamentMarkedAsOver(tournament, playerWhoWon);
            return tournament;
        }

        private bool FilledInOpenGame(Tournament tournament, Player playerWhoWon)
        {
            var openGame = tournament.Games.FirstOrDefault(game => game.IsOpen());
            if (openGame == null) return false;

            openGame.AddPlayer(playerWhoWon);
            return true;
        }

        private bool PutTournamentOnHoldForLaggingGame(Tournament tournament, Game wonGame, Player playerWhoWon)
        {
            var lowestLevelInProgressGame = tournament.Games
                .Where(game => game.IsInProgress())
                .OrderBy(game => game.Level)
                .FirstOrDefault();

            if (lowestLevelInProgressGame == null && lowestLevelInProgressGame.Level >= (wonGame.Level - 3))
                return false;

            wonGame = tournament.AddOnHoldGame(wonGame);
            return true;
        }

        private bool CreatedOpenGame(Tournament tournament, Game wonGame, Player playerWhoWon)
        {
            if (tournament.Games.Any(game => game.IsInProgress()))
            {
                tournament.AddGame(new Game(wonGame.Level + 1, new Player[] { playerWhoWon }));
                return true;
            }
            return false;
        }

        private bool TournamentMarkedAsOver(Tournament tournament, Player playerWhoWon)
        {
            if (tournament.Games.All(game => game.IsCompleted()))
            {
                tournament.Winner = playerWhoWon;
                return true;
            }
            return false;
        }

        private int NumberOfGamesForNumberOfDudes(int numberOfPlayersPerGame, int numberOfPlayers)
        {
            return (int)Math.Ceiling((double)numberOfPlayers / (double)numberOfPlayersPerGame);
        }
    }
}
