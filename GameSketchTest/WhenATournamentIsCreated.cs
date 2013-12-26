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
            _MockRepository.Setup(_ => _.Save(It.IsAny<Tournament>()))
                .Callback<Tournament>((t) => {
                    _SavedTournament = t;
                    _SaveCalled = true;
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
        public void SaveShouldHaveBeenCalled()
        {
            _SaveCalled.Should().BeTrue();
        }
    }
}
