using GameDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSketch
{
    //public class Game
    //{

    //    public Game(IEnumerable<Player> players)
    //    {
    //        this._players = players.ToList();
    //    }

    //    private List<Player> _players { get; set; }
    //    private Player _winner { get; set; }
    //    private string _name { get; set; }


    //    public IList<Player> Players()
    //    {
    //        return _players;
    //    }

    //    public bool IsOpen()
    //    {
    //        return _players.Count == 1;
    //    }

    //    public bool IsInProgress()
    //    {
    //        return !IsOpen() && !IsCompleted();
    //    }

    //    public bool IsCompleted()
    //    {
    //        return _winner != null;
    //    }

    //    public Player Winner()
    //    {
    //        return _winner;
    //    }

    //    public void MarkWinner(Player winner)
    //    {
    //        _winner = winner;
    //    }

    //    public void AddPlayer(Player player)
    //    {
    //        _players.Add(player);
    //    }
    //}

    //public class Player
    //{
    //    private string _name;

    //    public Player(string name)
    //    {
    //        this._name = name;
    //    }

    //    public string Name()
    //    {
    //        return _name;
    //    }
    //}

    //public class Tournament
    //{
    //    private string _name;
    //    private DateTime _startDate;
    //    private List<Player> _players;
    //    private List<Game> _games;
    //    private Player _winner;

    //    public Tournament()
    //    {
    //        _players = new List<Player>();
    //        _games = new List<Game>();
    //    }

    //    public void AddPlayer(Player player)
    //    {
    //        _players.Add(player);
    //    }

    //    public IList<Game> Games()
    //    {
    //        return _games;
    //    }

    //    public void AddGame(Game game)
    //    {
    //        _games.Add(game);
    //    }

    //    public IList<Player> Players()
    //    {
    //        return _players;
    //    }

    //    public Player Winner()
    //    {
    //        return _winner;
    //    }

    //    public bool IsCompleted()
    //    {
    //        return _winner != null;
    //    }

    //    internal void MarkWinner(Player player)
    //    {
    //        _winner = player;
    //    }
    //}

    public class TournamentService
    {
        const int NumberOfPlayersPerGame = 2;

        private ITournamentRepo _tournamentRepo;
        private IGameRepo _gameRepo;
        private IPlayerRepo _playerRepo;

        public TournamentService(
            ITournamentRepo tournamentRepo,
            IGameRepo gameRepo,
            IPlayerRepo playerRepo)
        {
            this._tournamentRepo = tournamentRepo;
            this._gameRepo = gameRepo;
            this._playerRepo = playerRepo;
        }

        public Tournament Start(Tournament tournament)
        {
            var players = tournament.Players;
            var numberOfGames = NumberOfGamesForNumberOfDudes(NumberOfPlayersPerGame, players.Count);
            for (int gameNumber = 0; gameNumber < numberOfGames; gameNumber++)
            {
                var newGame = new Game();
                newGame = _gameRepo.AddPlayers(newGame, players.Skip(gameNumber * NumberOfPlayersPerGame).Take(NumberOfPlayersPerGame));
                tournament = _tournamentRepo.AddGame(tournament, newGame);
                _gameRepo.Save(newGame);
            }
            foreach (var player in players)
            {
                _playerRepo.Save(player);
            }
            return _tournamentRepo.Save(tournament);
        }

        public Tournament GameWon(Tournament tournament, Game wonGame, Player playerWhoWon)
        {
            wonGame.Winner = playerWhoWon;
            var openGame = tournament.Games.FirstOrDefault(game => game.IsOpen());
            if (openGame != null)
            {
                openGame = _gameRepo.AddPlayer(openGame, playerWhoWon);
                openGame = _gameRepo.Save(openGame);
            }
            else if (tournament.Games.Any(game => game.IsInProgress()))
            {
                var newGame = _gameRepo.AddPlayer(new Game(), playerWhoWon);
                tournament = _tournamentRepo.AddGame(tournament, newGame);
                newGame = _gameRepo.Save(newGame);
                tournament = _tournamentRepo.Save(tournament);
            }
            else if (tournament.Games.All(game => game.IsCompleted()))
            {
                tournament.Winner = playerWhoWon;
                tournament = _tournamentRepo.Save(tournament);
            }

            return tournament;
        }

        private int NumberOfGamesForNumberOfDudes(int numberOfPlayersPerGame, int numberOfPlayers)
        {
            return (int)Math.Ceiling((double)numberOfPlayers / (double)numberOfPlayersPerGame);
        }
    }
}
