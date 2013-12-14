using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameSketch;
using FluentAssertions;
using GameDataLayer;

namespace GameSketchTest
{
    [TestClass]
    public class WhenATournamentStartsWithTwoPlayers
    {
        private Tournament _tournament;
        private Player[] _players = new Player[] { new Player("Dude1"), new Player("Dude2") };
        

        public WhenATournamentStartsWithTwoPlayers()
        {
            _tournament = new Tournament();
            _tournament.AddPlayer(_players[0]);
            _tournament.AddPlayer(_players[1]);
            _tournament = new TournamentService().Start(_tournament);
        }

        [TestMethod]
        public void ShouldHaveOneGame()
        {
            _tournament.Games().Count.Should().Be(1);
        }

        [TestMethod]
        public void GameShouldHaveTwoDudes()
        {
            _tournament.Games()[0].Players().Should().Contain(_players[0]).And.Contain(_players[1]);
        }
    }
}
