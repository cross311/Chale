using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSketch
{
    public class Game
    {

        public Game(IEnumerable<Dude> dudes)
        {
            this._dudes = dudes.ToList();
        }

        private List<Dude> _dudes { get; set; }
        private Dude _winner { get; set; }
        private string _name { get; set; }


        public IList<Dude> Dudes()
        {
            return _dudes;
        }

        public bool IsOpen()
        {
            return _dudes.Count == 1;
        }

        public bool IsInProgress()
        {
            return !IsOpen() && !IsCompleted();
        }

        public Dude Winner()
        {
            return _winner;
        }

        public void MarkWinner(Dude winner)
        {
            _winner = winner;
        }

        public bool IsCompleted()
        {
            return _winner != null;
        }

        public void AddDude(Dude dude)
        {
            _dudes.Add(dude);
        }
    }

    public class Dude
    {
        private string _name;

        public Dude(string name)
        {
            this._name = name;
        }

    }

    public class Tournament
    {
        private string _name;
        private DateTime _startDate;
        private List<Dude> _dudes;
        private List<Game> _games;

        public Tournament()
        {
            _dudes = new List<Dude>();
            _games = new List<Game>();
        }

        public void AddDude(Dude dude)
        {
            _dudes.Add(dude);
        }

        public IList<Game> Games()
        {
            return _games;
        }

        public void AddGame(Game game)
        {
            _games.Add(game);
        }

        public IList<Dude> Dudes()
        {
            return _dudes;
        }
    }

    public class GameService
    {

        public Game MarkWinner(Game _game, Dude _winner)
        {
            _game.MarkWinner(_winner);
            return _game;
        }
    }

    public class TournamentService
    {
        public Tournament Start(Tournament tournament)
        {
            var dudes = tournament.Dudes();
            var numberPerGame = 2;
            var numberOfGames = NumberOfGamesForNumberOfDudes(numberPerGame, dudes.Count);
            for (int gameNumber = 0; gameNumber < numberOfGames; gameNumber++)
            {
                tournament.AddGame(new Game(dudes.Skip(gameNumber * numberPerGame).Take(numberPerGame)));
            }
            return tournament;
        }

        private int NumberOfGamesForNumberOfDudes(int numberOfDudesPerGame, int numberOfDudes)
        {
            return (int)Math.Ceiling((double)numberOfDudes / (double)numberOfDudesPerGame);
        }

        public Tournament GameWon(Tournament tournament, Game wonGame, Dude dudeWhoWon)
        {
            wonGame.MarkWinner(dudeWhoWon);
            var openGame = tournament.Games().FirstOrDefault(game => game.IsOpen());
            openGame.AddDude(dudeWhoWon);
            return tournament;
        }
    }
}
