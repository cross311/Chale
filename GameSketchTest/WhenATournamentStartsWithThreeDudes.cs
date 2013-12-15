using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameSketch;
using FluentAssertions;
using System.Linq;

namespace GameSketchTest
{
    [TestClass]
    public class WhenATournamentStartsWithThreeDudes
    {
        private Dude[] _dudes = new Dude[]{
            new Dude("Dude1"),
            new Dude("Dude2"),
            new Dude("Dude3")};

        private Tournament _tournament;

        public WhenATournamentStartsWithThreeDudes()
        {
            _tournament = new Tournament();
            _tournament.AddDude(_dudes[0]);
            _tournament.AddDude(_dudes[1]);
            _tournament.AddDude(_dudes[2]);

            _tournament = new TournamentService().Start(_tournament);
        }

        [TestMethod]
        public void ShouldBeTwoGames()
        {
            _tournament.Games().Count.Should().Be(2);
        }

        [TestMethod]
        public void OneGameShouldBeOpen()
        {
            _tournament.Games().Should().Contain((game) => game.IsOpen());
        }

        [TestMethod]
        public void OneGameShouldBeInProgress()
        {
            _tournament.Games().Should().Contain((game) => game.IsInProgress());
        }

        [TestMethod]
        public void OneGameShouldNotBeInProgress()
        {
            _tournament.Games().Where(game => !game.IsInProgress()).Count().Should().Be(1);
        }

        [TestMethod]
        public void TwoGamesShouldNotBeCompleted()
        {
            _tournament.Games().Where(game => !game.IsCompleted()).Count().Should().Be(2);
        }
    }
}
