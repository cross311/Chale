using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameSketch;
using System.Linq;
using FluentAssertions;

namespace GameSketchTest
{
    [TestClass]
    public class WhenALargeOddTournamentCompletes
    {
        private Random _rand = new Random();

        private int NumberOfPlayers = 49;
        private Tournament _tournament;

        public WhenALargeOddTournamentCompletes()
        {
            _tournament = new Tournament();

            for (int playerNumber = 1; playerNumber <= NumberOfPlayers; playerNumber++)
            {
                _tournament.AddPlayer(new Player(string.Format("Dude{0}", playerNumber)));
            }

            var tournamentSvc = new TournamentService();

            _tournament = tournamentSvc.Start(_tournament);

            Game gameToWin;
            int winner = 0;
            while (null !=
                (gameToWin = _tournament.Games().Where(game => game.IsInProgress()).FirstOrDefault()))
            {
                winner = (winner == 0 ? 1 : 0);
                _tournament = tournamentSvc.GameWon(_tournament, gameToWin, gameToWin.Players()[winner]);
            }
        }

        [TestMethod]
        public void ShouldHaveHadOneLessGameThenNumberOfPlayers()
        {
            _tournament.Games().Count.Should().Be(NumberOfPlayers - 1);
        }

        [TestMethod]
        public void ShouldHaveAWinner()
        {
            _tournament.Winner().Name().Should().Be("Dude35");
        }
    }
}
