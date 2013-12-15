using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameSketch;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;

namespace GameSketchTest
{
    [TestClass]
    public class WhenAGameHasAWinnerInATournamentWithFourDudes
    {
        private List<Dude> _dudes = new List<Dude>{
            new Dude("Dude1"),
            new Dude("Dude2"),
            new Dude("Dude3"),
            new Dude("Dude4")
        };

        private Dude _wonDude;
        private Game _wonGame;
        private Tournament _tournament;
        private Mock<IRepository<Tournament>> _MockRepository;

        public WhenAGameHasAWinnerInATournamentWithFourDudes()
        {
            _tournament = new Tournament();
            _tournament.AddDude(_dudes[0]);
            _tournament.AddDude(_dudes[1]);
            _tournament.AddDude(_dudes[2]);
            _tournament.AddDude(_dudes[3]);

            _wonGame = new Game(_dudes.Take(2));
            var secondGame = new Game(_dudes.Skip(2).Take(2));

            _tournament.AddGame(_wonGame);
            _tournament.AddGame(secondGame);

            _wonDude = _wonGame.Dudes()[0];

            _MockRepository = new Mock<IRepository<Tournament>>();

            _tournament = new TournamentService(_MockRepository.Object).GameWon(_tournament, _wonGame, _wonDude);
        }

        [TestMethod]
        public void ThereShouldBeThreeGames()
        {
            _tournament.Games().Count.Should().Be(3);
        }

        [TestMethod]
        public void AGameShouldBeOpen()
        {
            _tournament.Games().Should().Contain(game => game.IsOpen());
        }

        [TestMethod]
        public void TheOpenGameShouldContainTheWinningDude()
        {
            _tournament.Games().Single(game => game.IsOpen()).Dudes().Should().Contain(_wonDude);
        }
    }
}
