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
                tournament.AddGame(new Game(players.Skip(gameNumber * NumberOfPlayersPerGame).Take(NumberOfPlayersPerGame)));
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
                tournament.AddGame(new Game(new Player[] { playerWhoWon }));
            else if (tournament.Games.All(game => game.IsCompleted()))
                tournament.Winner = playerWhoWon;

            return tournament;
        }

        private int NumberOfGamesForNumberOfDudes(int numberOfPlayersPerGame, int numberOfPlayers)
        {
            return (int)Math.Ceiling((double)numberOfPlayers / (double)numberOfPlayersPerGame);
        }
    }
}
