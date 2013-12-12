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

        public string Name()
        {
            return _name;
        }
    }

    public class Tournament
    {
        private string _name;
        private DateTime _startDate;
        private List<Dude> _dudes;
        private List<Game> _games;
        private Dude _winner;

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

        public Dude Winner()
        {
            return _winner;
        }

        public bool IsCompleted()
        {
            return _winner != null;
        }

        internal void MarkWinner(Dude dude)
        {
            _winner = dude;
        }
    }

    public class TournamentService
    {
        const int NumberOfDudesPerGame = 2;

        public Tournament Start(Tournament tournament)
        {
            var dudes = tournament.Dudes();
            var numberOfGames = NumberOfGamesForNumberOfDudes(NumberOfDudesPerGame, dudes.Count);
            for (int gameNumber = 0; gameNumber < numberOfGames; gameNumber++)
            {
                tournament.AddGame(new Game(dudes.Skip(gameNumber * NumberOfDudesPerGame).Take(NumberOfDudesPerGame)));
            }
            return tournament;
        }

        public Tournament GameWon(Tournament tournament, Game wonGame, Dude dudeWhoWon)
        {
            wonGame.MarkWinner(dudeWhoWon);
            var openGame = tournament.Games().FirstOrDefault(game => game.IsOpen());
            if (openGame != null)
                openGame.AddDude(dudeWhoWon);
            else if (tournament.Games().Any(game => game.IsInProgress()))
                tournament.AddGame(new Game(new Dude[] { dudeWhoWon }));
            else if (tournament.Games().All(game => game.IsCompleted()))
                tournament.MarkWinner(dudeWhoWon);

            return tournament;
        }

        private int NumberOfGamesForNumberOfDudes(int numberOfDudesPerGame, int numberOfDudes)
        {
            return (int)Math.Ceiling((double)numberOfDudes / (double)numberOfDudesPerGame);
        }
    }
}
