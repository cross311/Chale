using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameSketch;
using System.Linq;
using FluentAssertions;
using GameDataLayer;
using Moq;

namespace GameSketchTest
{
    [TestClass]
    public class WhenALargeOddTournamentCompletes
    {
        private Random _rand = new Random();

        private int NumberOfPlayers = 49;
        private Tournament _tournament;
        private Mock<IRepository<Tournament>> _MockRepository;

        public WhenALargeOddTournamentCompletes()
        {
            _tournament = new Tournament();
            _MockRepository = new Mock<IRepository<Tournament>>();
            _MockRepository.Setup(_ => _.Save(It.IsAny<Tournament>())).Returns(_tournament);

            for (int playerNumber = 1; playerNumber <= NumberOfPlayers; playerNumber++)
            {
                _tournament.AddPlayer(new Player(string.Format("Dude{0}", playerNumber)));
            }

            var tournamentSvc = new TournamentService(_MockRepository.Object);

            _tournament = tournamentSvc.Start(_tournament);

            Game gameToWin;
            int winner = 0;
            while (null !=
                (gameToWin = _tournament.Games.Where(game => game.IsInProgress()).FirstOrDefault()))
            {
                winner = (winner == 0 ? 1 : 0);
                _tournament = tournamentSvc.GameWon(_tournament, gameToWin, gameToWin.Players[winner]);
            }
        }

        [TestMethod]
        public void ShouldHaveHadOneLessGameThenNumberOfPlayers()
        {
            _tournament.Games.Count.Should().Be(NumberOfPlayers - 1);
        }

        [TestMethod]
        public void ShouldHaveAWinner()
        {
            _tournament.Winner.Name.Should().Be("Dude35");
        }
    }
}
