using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameSketch;
using FluentAssertions;
using System.Linq;
using GameDataLayer;
using Moq;

namespace GameSketchTest
{
    [TestClass]
    public class WhenATournamentStartsWithThreePlayers
    {
        private Player[] _players = new Player[]{
            new Player("Dude1"),
            new Player("Dude2"),
            new Player("Dude3")};

        private Tournament _tournament;
        private Mock<IRepository<Tournament>> _MockRepository;

        public WhenATournamentStartsWithThreePlayers()
        {

            _tournament = new Tournament();
            _tournament.AddPlayer(_players[0]);
            _tournament.AddPlayer(_players[1]);
            _tournament.AddPlayer(_players[2]);

            _MockRepository = new Mock<IRepository<Tournament>>();
            _MockRepository.Setup(_ => _.Save(It.IsAny<Tournament>())).Returns(_tournament);

            _tournament = new TournamentService(_MockRepository.Object).Start(_tournament);
        }

        [TestMethod]
        public void ShouldBeTwoGames()
        {
            _tournament.Games.Count.Should().Be(2);
        }

        [TestMethod]
        public void OneGameShouldBeOpen()
        {
            _tournament.Games.Should().Contain((game) => game.IsOpen());
        }

        [TestMethod]
        public void OneGameShouldBeInProgress()
        {
            _tournament.Games.Should().Contain((game) => game.IsInProgress());
        }

        [TestMethod]
        public void OneGameShouldNotBeInProgress()
        {
            _tournament.Games.Where(game => !game.IsInProgress()).Count().Should().Be(1);
        }

        [TestMethod]
        public void TwoGamesShouldNotBeCompleted()
        {
            _tournament.Games.Where(game => !game.IsCompleted()).Count().Should().Be(2);
        }
    }
}
