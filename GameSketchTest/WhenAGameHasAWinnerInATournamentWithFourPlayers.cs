using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameSketch;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GameDataLayer;
using Moq;

namespace GameSketchTest
{
    [TestClass]
    public class WhenAGameHasAWinnerInATournamentWithFourPlayers
    {
        private List<Player> _players = new List<Player>{
            new Player("Dude1"),
            new Player("Dude2"),
            new Player("Dude3"),
            new Player("Dude4")
        };

        private Player _wonPlayer;
        private Game _wonGame;
        private Tournament _tournament;
        private Mock<IRepository<Tournament>> _MockRepository;

        public WhenAGameHasAWinnerInATournamentWithFourPlayers()
        {
            _MockRepository = new Mock<IRepository<Tournament>>();
            _tournament = new Tournament();
            _tournament.AddPlayer(_players[0]);
            _tournament.AddPlayer(_players[1]);
            _tournament.AddPlayer(_players[2]);
            _tournament.AddPlayer(_players[3]);

            _wonGame = new Game(_players.Take(2));
            var secondGame = new Game(_players.Skip(2).Take(2));

            _tournament.AddGame(_wonGame);
            _tournament.AddGame(secondGame);

            _wonPlayer = _wonGame.Players[0];

            _tournament = new TournamentService(_MockRepository.Object).GameWon(_tournament, _wonGame, _wonPlayer);
        }

        [TestMethod]
        public void ThereShouldBeThreeGames()
        {
            _tournament.Games.Count.Should().Be(3);
        }

        [TestMethod]
        public void AGameShouldBeOpen()
        {
            _tournament.Games.Should().Contain(game => game.IsOpen());
        }

        [TestMethod]
        public void TheOpenGameShouldContainTheWinningDude()
        {
            _tournament.Games.Single(game => game.IsOpen()).Players.Should().Contain(_wonPlayer);
        }
    }
}
