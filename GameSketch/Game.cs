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
            var numberOfGames = NumberOfFullGamesForNumberOfDudes(NumberOfPlayersPerGame, players.Count);
            var startingLevel = tournament.CurrentLevel() + 1;
            for (int gameNumber = 0; gameNumber < numberOfGames; gameNumber++)
            {
                tournament.AddGame(new Game(startingLevel, players.Skip(gameNumber * NumberOfPlayersPerGame).Take(NumberOfPlayersPerGame)));
            }

            if (ShouldStartWithOpenGame(NumberOfPlayersPerGame, players.Count))
            {
                var openGamesLevel = startingLevel + 1;
                tournament.AddGame(new Game(openGamesLevel, players.Skip(numberOfGames * NumberOfPlayersPerGame).Take(NumberOfPlayersPerGame)));
            }

            var result = _tournamentRepo.Save(tournament);
            return result;
        }

        public Tournament GameWon(Tournament tournament, Game wonGame, Player playerWhoWon)
        {
            wonGame.Winner = playerWhoWon;

            return GameWonLogic(tournament, wonGame, playerWhoWon);
        }

        private Tournament GameWonLogic(Tournament tournament, Game wonGame, Player playerWhoWon)
        {
            if (!FilledInOpenGame(tournament, playerWhoWon))
                if (!PutTournamentOnHoldForLaggingGame(tournament, wonGame, playerWhoWon))
                    if (!LeftTournamentOnHoldBecauseOfAnotherLaggingGame(tournament, wonGame, playerWhoWon))
                        if (!ResumedOnHoldTournamentDueToFinalLaggingGameFinishing(tournament, wonGame, playerWhoWon))
                            if (!CreatedOpenGame(tournament, wonGame, playerWhoWon))
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
            if (IsThereALaggingGame(wonGame.Level - 1, tournament.Games))
            {
                wonGame = tournament.AddOnHoldGame(wonGame);
                return true;
            }
            return false;
        }

        private bool LeftTournamentOnHoldBecauseOfAnotherLaggingGame(Tournament tournament, Game wonGame, Player playerWhoWon)
        {
            if (!tournament.IsOnHold()) return false;
            if (IsThereALaggingGame(tournament.CurrentLevel() - 1, tournament.Games.Where(g => g != wonGame)))
            {
                wonGame = tournament.AddOnHoldGame(wonGame);
                return true;
            }
            return false;
        }

        private bool ResumedOnHoldTournamentDueToFinalLaggingGameFinishing(Tournament tournament, Game wonGame, Player playerWhoWon)
        {
            if (!tournament.IsOnHold()) return false;
            wonGame.OnHold = true;
            var wonGamesOnHold = tournament.OnHoldGames;

            var wonGamesFromLowestToHighestLevel = wonGamesOnHold.OrderBy(g => g.Level).ToList();
            var highestGameIndex = wonGamesFromLowestToHighestLevel.Count - 1;
            var newGamesLevel = wonGamesFromLowestToHighestLevel[highestGameIndex].Level + 1;

            for (int lowerLevelGameIndex = 0; lowerLevelGameIndex <= highestGameIndex / 2; lowerLevelGameIndex++)
            {
                var higherLevelGameIndex = highestGameIndex - lowerLevelGameIndex;
                var higherLevelWinner = wonGamesFromLowestToHighestLevel[highestGameIndex].Winner;
                var lowerLevelWinner = wonGamesFromLowestToHighestLevel[lowerLevelGameIndex].Winner;
                if (higherLevelGameIndex != lowerLevelGameIndex)
                {
                    tournament.AddGame(new Game(newGamesLevel, new Player[] { higherLevelWinner, lowerLevelWinner }));
                }
                else
                {
                    tournament.AddGame(new Game(newGamesLevel, new Player[] { lowerLevelWinner }));
                }
            }
            tournament.MarkAsResumed();

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

        private int NumberOfFullGamesForNumberOfDudes(int numberOfPlayersPerGame, int numberOfPlayers)
        {
            return numberOfPlayers / numberOfPlayersPerGame;
        }
        private bool ShouldStartWithOpenGame(int numberOfPlayersPerGame, int numberOfPlayers)
        {
            return (numberOfPlayers % numberOfPlayersPerGame) != 0;
        }

        private bool IsThereALaggingGame(int laggingLevel, IEnumerable<Game> games)
        {
            var lowestLevelInProgressGame = games
                .Where(game => game.IsInProgress())
                .OrderBy(game => game.Level)
                .FirstOrDefault();

            return lowestLevelInProgressGame != null && IsLaggingGame(laggingLevel, lowestLevelInProgressGame);
        }

        private bool IsLaggingGame(int laggingLevel, Game suspectedLaggingGame)
        {
            return suspectedLaggingGame.Level <= laggingLevel;
        }

        public Tournament Create(string name, string description)
        {
            var newTournament = new Tournament()
            {
                Name = name,
                Description = description
            };

            newTournament = _tournamentRepo.Save(newTournament);
            return newTournament;
        }
    }
}
