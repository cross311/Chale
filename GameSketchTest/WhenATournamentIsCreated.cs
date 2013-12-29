using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameSketch;
using GameDataLayer;
using FluentAssertions;
using Moq;

namespace GameSketchTest
{
    [TestClass]
    public class WhenATournamentIsCreated
    {
        private bool _AddCalled;
        private bool _SaveCalled;
        private TournamentService _service;
        private Mock<IRepository<Tournament>> _MockRepository;

        private string _Name;
        private string _Description;
        private Tournament _SavedTournament;

        public WhenATournamentIsCreated()
        {
            _SavedTournament = new Tournament();
            _MockRepository = new Mock<IRepository<Tournament>>();
            _MockRepository.Setup(_ => _.AddNew(It.IsAny<Tournament>()))
                .Callback<Tournament>((t) => {
                    _SavedTournament = t;
                    _AddCalled = true;
                });

            _service = new TournamentService(_MockRepository.Object);

            _Name = "Test Name";
            _Description = "Test Description";

            _service.Create(_Name, _Description);
        }

        [TestMethod]
        public void NameShouldBeFilledIn()
        {
            _SavedTournament.Name.Should().Be(_Name);
        }

        [TestMethod]
        public void DescriptionShouldBeFilledIn()
        {
            _SavedTournament.Description.Should().Be(_Description);
        }

        [TestMethod]
        public void StartDateShouldBeNull()
        {
            _SavedTournament.StartDate.Should().Be(default(DateTime?));
        }

        [TestMethod]
        public void TournamentShouldHaveBeenAddedToRepository()
        {
            _AddCalled.Should().BeTrue();
        }

        [TestMethod]
        public void SaveShouldHaveBeenCalled()
        {
            _MockRepository.Verify(_ => _.SaveChanges());
        }
    }
}
