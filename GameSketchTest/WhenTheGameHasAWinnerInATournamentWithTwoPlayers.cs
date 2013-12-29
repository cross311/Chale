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
    public class WhenTheGameHasAWinnerInATournamentWithTwoPlayers
    {
        private List<Player> _players = new List<Player>{
            new Player("Dude1"),
            new Player("Dude2")};

        private Player _wonPlayer;
        private Game _game;
        private Tournament _tournament;
        private Mock<IRepository<Tournament>> _MockRepository;

        public WhenTheGameHasAWinnerInATournamentWithTwoPlayers()
        {
            _tournament = new Tournament();
            _tournament.AddPlayer(_players[0]);
            _tournament.AddPlayer(_players[1]);

            _game = new Game(1, _players);

            _tournament.AddGame(_game);

            _wonPlayer = _game.Players[0];
            
            _MockRepository = new Mock<IRepository<Tournament>>();

            _tournament = new TournamentService(_MockRepository.Object).GameWon(_tournament, _game, _wonPlayer);
        }

        [TestMethod]
        public void TheGameShouldBeMarkedAsCompleted()
        {
            _game.IsCompleted().Should().Be(true);
        }

        [TestMethod]
        public void TheGameShouldHaveAWinner()
        {
            _game.Winner.Should().Be(_wonPlayer);
        }

        [TestMethod]
        public void TheTournamentShouldHaveAWinner()
        {
            _tournament.Winner.Should().Be(_wonPlayer);
        }

        [TestMethod]
        public void TheTournamentShouldBeCompleted()
        {
            _tournament.IsCompleted().Should().Be(true);
        }

        [TestMethod]
        public void SaveShouldHaveBeenCalled()
        {
            _MockRepository.Verify(_ => _.SaveChanges());
        }
    }
}
