using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameDataLayer;
using Moq;
using GameSketch;
using FluentAssertions;

namespace GameSketchTest
{
    [TestClass]
    public class WhenAPlayerIsAddedToANotStartedTournament
    {
        private TournamentService _service;
        private Mock<IRepository<Tournament>> _MockRepository;

        private string _Name;
        private Tournament _SavedTournament;
        private Player _AddedPlayer;

        public WhenAPlayerIsAddedToANotStartedTournament()
        {
            _SavedTournament = new Tournament();
            _MockRepository = new Mock<IRepository<Tournament>>();

            _service = new TournamentService(_MockRepository.Object);

            _Name = "Test Player Name";
            _AddedPlayer = new Player(_Name);

            _service.AddPlayer(_SavedTournament, _AddedPlayer);
        }


        [TestMethod]
        public void TournamentShouldHavePlayerInPlayers()
        {
            _SavedTournament.Players.Should().Contain(_AddedPlayer);
        }

        [TestMethod]
        public void AddedPlayerShouldHaveReferenceToTournament()
        {
            _AddedPlayer.Tournament.Should().Be(_SavedTournament);
        }

        [TestMethod]
        public void SaveShouldHaveBeenCalled()
        {
            _MockRepository.Verify(_ => _.SaveChanges());
        }
    }
}
