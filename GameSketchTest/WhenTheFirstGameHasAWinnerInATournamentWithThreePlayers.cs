using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameSketch;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using GameDataLayer;
using Moq;

namespace GameSketchTest
{
    [TestClass]
    public class WhenTheFirstGameHasAWinnerInATournamentWithThreePlayers
    {
        private List<Player> _players = new List<Player>{
            new Player("Dude1"),
            new Player("Dude2"),
            new Player("Dude3")};

        private Player _wonPlayer;
        private Game _wonGame;
        private Game _secondGame;
        private Tournament _tournament;
        private Mock<IRepository<Tournament>> _MockRepository;

        public WhenTheFirstGameHasAWinnerInATournamentWithThreePlayers()
        {
            _tournament = new Tournament();
            _tournament.AddPlayer(_players[0]);
            _tournament.AddPlayer(_players[1]);
            _tournament.AddPlayer(_players[2]);

            _wonGame = new Game(1, _players.Take(2));
            _secondGame = new Game(1, _players.Skip(2).Take(1));

            _tournament.AddGame(_wonGame);
            _tournament.AddGame(_secondGame);

            _wonPlayer = _wonGame.Players[0];

            _MockRepository = new Mock<IRepository<Tournament>>();

            _tournament = new TournamentService(_MockRepository.Object).GameWon(_tournament, _wonGame, _wonPlayer);
        }

        [TestMethod]
        public void TheSecondGameShouldBeInProgress()
        {
            _secondGame.IsInProgress().Should().Be(true);
        }

        [TestMethod]
        public void TheSecondGameShouldHaveTheWinnerFromTheWonGame()
        {
            _secondGame.Players.Should().Contain(_wonPlayer);
        }

        [TestMethod]
        public void SaveShouldHaveBeenCalled()
        {
            _MockRepository.Verify(_ => _.SaveChanges());
        }
    }
}
