using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSketch
{
    public class Game
    {

        public Game(IEnumerable<Player> players)
        {
            this._players = players.ToList();
        }

        private List<Player> _players { get; set; }
        private Player _winner { get; set; }
        private string _name { get; set; }


        public IList<Player> Players()
        {
            return _players;
        }

        public bool IsOpen()
        {
            return _players.Count == 1;
        }

        public bool IsInProgress()
        {
            return !IsOpen() && !IsCompleted();
        }

        public Player Winner()
        {
            return _winner;
        }

        public void MarkWinner(Player winner)
        {
            _winner = winner;
        }

        public bool IsCompleted()
        {
            return _winner != null;
        }

        public void AddPlayer(Player player)
        {
            _players.Add(player);
        }
    }

    public class Player
    {
        private string _name;

        public Player(string name)
        {
            this._name = name;
        }

        public string Name()
        {
            return _name;
        }
    }

    public class Tournament
    {
        private string _name;
        private DateTime _startDate;
        private List<Player> _players;
        private List<Game> _games;
        private Player _winner;

        public Tournament()
        {
            _players = new List<Player>();
            _games = new List<Game>();
        }

        public void AddPlayer(Player player)
        {
            _players.Add(player);
        }

        public IList<Game> Games()
        {
            return _games;
        }

        public void AddGame(Game game)
        {
            _games.Add(game);
        }

        public IList<Player> Players()
        {
            return _players;
        }

        public Player Winner()
        {
            return _winner;
        }

        public bool IsCompleted()
        {
            return _winner != null;
        }

        internal void MarkWinner(Player player)
        {
            _winner = player;
        }
    }

    public class TournamentService
    {
        const int NumberOfPlayersPerGame = 2;

        public Tournament Start(Tournament tournament)
        {
            var players = tournament.Players();
            var numberOfGames = NumberOfGamesForNumberOfDudes(NumberOfPlayersPerGame, players.Count);
            for (int gameNumber = 0; gameNumber < numberOfGames; gameNumber++)
            {
                tournament.AddGame(new Game(players.Skip(gameNumber * NumberOfPlayersPerGame).Take(NumberOfPlayersPerGame)));
            }
            return tournament;
        }

        public Tournament GameWon(Tournament tournament, Game wonGame, Player playerWhoWon)
        {
            wonGame.MarkWinner(playerWhoWon);
            var openGame = tournament.Games().FirstOrDefault(game => game.IsOpen());
            if (openGame != null)
                openGame.AddPlayer(playerWhoWon);
            else if (tournament.Games().Any(game => game.IsInProgress()))
                tournament.AddGame(new Game(new Player[] { playerWhoWon }));
            else if (tournament.Games().All(game => game.IsCompleted()))
                tournament.MarkWinner(playerWhoWon);

            return tournament;
        }

        private int NumberOfGamesForNumberOfDudes(int numberOfPlayersPerGame, int numberOfPlayers)
        {
            return (int)Math.Ceiling((double)numberOfPlayers / (double)numberOfPlayersPerGame);
        }
    }
}
