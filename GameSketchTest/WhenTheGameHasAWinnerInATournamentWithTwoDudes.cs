using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameSketch;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;

namespace GameSketchTest
{
    [TestClass]
    public class WhenTheGameHasAWinnerInATournamentWithTwoDudes
    {
        private List<Dude> _dudes = new List<Dude>{
            new Dude("Dude1"),
            new Dude("Dude2")};

        private Dude _wonDude;
        private Game _game;
        private Tournament _tournament;

        public WhenTheGameHasAWinnerInATournamentWithTwoDudes()
        {
            _tournament = new Tournament();
            _tournament.AddDude(_dudes[0]);
            _tournament.AddDude(_dudes[1]);

            _game = new Game(_dudes);

            _tournament.AddGame(_game);

            _wonDude = _game.Dudes()[0];

            _tournament = new TournamentService().GameWon(_tournament, _game, _wonDude);
        }

        [TestMethod]
        public void TheGameShouldBeMarkedAsCompleted()
        {
            _game.IsCompleted().Should().Be(true);
        }

        [TestMethod]
        public void TheGameShouldHaveAWinner()
        {
            _game.Winner().Should().Be(_wonDude);
        }

        [TestMethod]
        public void TheTournamentShouldHaveAWinner()
        {
            _tournament.Winner().Should().Be(_wonDude);
        }

        [TestMethod]
        public void TheTournamentShouldBeCompleted()
        {
            _tournament.IsCompleted().Should().Be(true);
        }
    }
}
