﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameSketch;
using FluentAssertions;
using Moq;
using GameDataLayer;

namespace GameSketchTest
{
    [TestClass]
    public class WhenATournamentStarts
    {
        private Tournament _tournament;
        private Mock<IRepository<Tournament>> _MockRepository;

        public WhenATournamentStarts()
        {
            _tournament = new Tournament();

            _tournament.AddPlayer(new Player("fred"));
            _tournament.AddPlayer(new Player("barney"));

            _MockRepository = new Mock<IRepository<Tournament>>();
            _tournament = new TournamentService(_MockRepository.Object).Start(_tournament);
        }

        [TestMethod]
        public void SaveShouldHaveBeenCalled()
        {
            _MockRepository.Verify(_ => _.SaveChanges());
        }

        // TODO: implement this test

        //[TestMethod]
        //[ExpectedException(typeof(InvalidOperationException))]
        public void NoNewEntriesAreAccepted()
        {
            _tournament.AddPlayer(new Player("late entry dude"));
        }
    }
}