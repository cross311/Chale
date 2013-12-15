using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameSketch;
using FluentAssertions;

namespace GameSketchTest
{
    [TestClass]
    public class WhenATournamentStartsWithTwoDudes
    {
        private Tournament _tournament;
        private Dude[] _dudes = new Dude[] { new Dude("Dude1"), new Dude("Dude2") };
        

        public WhenATournamentStartsWithTwoDudes()
        {
            _tournament = new Tournament();
            _tournament.AddDude(_dudes[0]);
            _tournament.AddDude(_dudes[1]);
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
            _tournament.Games()[0].Dudes().Should().Contain(_dudes[0]).And.Contain(_dudes[1]);
        }
    }
}
