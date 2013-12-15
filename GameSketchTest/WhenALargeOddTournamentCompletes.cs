using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameSketch;
using System.Linq;
using FluentAssertions;
using Moq;

namespace GameSketchTest
{
    [TestClass]
    public class WhenALargeOddTournamentCompletes
    {
        private Random _rand = new Random();

        private int NumberOfDudes = 49;
        private Tournament _tournament;
        private Mock<IRepository<Tournament>> _MockRepository;

        public WhenALargeOddTournamentCompletes()
        {
            _tournament = new Tournament();

            for (int dudeNumber = 1; dudeNumber <= NumberOfDudes; dudeNumber++)
            {
                _tournament.AddDude(new Dude(string.Format("Dude{0}", dudeNumber)));
            }

            _MockRepository = new Mock<IRepository<Tournament>>();


            var tournamentSvc = new TournamentService(_MockRepository.Object);

            _tournament = tournamentSvc.Start(_tournament);

            Game gameToWin;
            int winner = 0;
            while (null !=
                (gameToWin = _tournament.Games().Where(game => game.IsInProgress()).FirstOrDefault()))
            {
                winner = (winner == 0 ? 1 : 0);
                _tournament = tournamentSvc.GameWon(_tournament, gameToWin, gameToWin.Dudes()[winner]);
            }
        }

        [TestMethod]
        public void ShouldHaveHadOneLessGameThenNumberOfPlayers()
        {
            _tournament.Games().Count.Should().Be(NumberOfDudes - 1);
        }

        [TestMethod]
        public void ShouldHaveAWinner()
        {
            _tournament.Winner().Name().Should().Be("Dude35");
        }
    }
}
