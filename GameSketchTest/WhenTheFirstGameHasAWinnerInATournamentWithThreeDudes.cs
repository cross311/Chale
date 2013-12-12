using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameSketch;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;

namespace GameSketchTest
{
    [TestClass]
    public class WhenTheFirstGameHasAWinnerInATournamentWithThreeDudes
    {
        private List<Dude> _dudes = new List<Dude>{
            new Dude("Dude1"),
            new Dude("Dude2"),
            new Dude("Dude3")};

        private Dude _wonDude;
        private Game _wonGame;
        private Game _secondGame;
        private Tournament _tournament;

        public WhenTheFirstGameHasAWinnerInATournamentWithThreeDudes()
        {
            _tournament = new Tournament();
            _tournament.AddDude(_dudes[0]);
            _tournament.AddDude(_dudes[1]);
            _tournament.AddDude(_dudes[2]);

            _wonGame = new Game(_dudes.Take(2));
            _secondGame = new Game(_dudes.Skip(2).Take(1));

            _tournament.AddGame(_wonGame);
            _tournament.AddGame(_secondGame);

            _wonDude = _wonGame.Dudes()[0];

            _tournament = new TournamentService().GameWon(_tournament, _wonGame, _wonDude);
        }

        [TestMethod]
        public void TheWonGameShouldBeMarkedAsCompleted()
        {
            _wonGame.IsCompleted().Should().Be(true);
        }

        [TestMethod]
        public void TheWonGameShouldHaveAWinner()
        {
            _wonGame.Winner().Should().Be(_wonDude);
        }

        [TestMethod]
        public void TheSecondGameShouldBeInProgress()
        {
            _secondGame.IsInProgress().Should().Be(true);
        }

        [TestMethod]
        public void TheSecondGameShouldHaveTheWinnerFromTheWonGame()
        {
            _secondGame.Dudes().Should().Contain(_wonDude);
        }
    }
}
