using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameDataLayer;
using System.Linq;
using GameSketch;
using Moq;
using System.Collections.Generic;
using FluentAssertions;

namespace GameSketchTest
{
    [TestClass]
    public class WhenALaggingGameIsStillInProgressAndOnTrackGamesAreWon
    {
        private Tournament _tournament;
        private TournamentService _tournamentService;
        private Mock<IRepository<Tournament>> _MockRepository;

        private List<Player> _players = new List<Player>()
        {
            new Player("player1"),
            new Player("player2"),
            new Player("player3"),
            new Player("player4"),
            new Player("player5"),
            new Player("player6"),
            new Player("player7"),
            new Player("player8"),
            new Player("player9"),
            new Player("player10"),
            new Player("player11"),
            new Player("player12"),
            new Player("player13"),
            new Player("player14")
        };

        private Game _NotPlayedGame;

        public WhenALaggingGameIsStillInProgressAndOnTrackGamesAreWon()
        {
            _tournament = new Tournament();
            _MockRepository = new Mock<IRepository<Tournament>>();
            _tournamentService = new TournamentService(_MockRepository.Object);

            for(int pIndex = 0; pIndex < _players.Count; pIndex++)
                _tournament.AddPlayer(_players[pIndex]);

            _tournamentService.Start(_tournament);
            _NotPlayedGame = _tournament.Games[0];

            _tournament.Games
                .Where(g => g != _NotPlayedGame)
                .ToList().ForEach(g => _tournamentService.GameWon(_tournament, g, g.Players[0]));

            _tournament.Games
                .Where(g => g != _NotPlayedGame && g.IsInProgress())
                .ToList().ForEach(g => _tournamentService.GameWon(_tournament, g, g.Players[0]));
        }

        [TestMethod]
        public void NextLevelGamesShouldNotBeInProgress()
        {
            var gamesInProgress = _tournament.Games
                .Where(g => g.IsInProgress() && g.Level > _NotPlayedGame.Level);

            gamesInProgress.Should().BeEmpty();
        }

        [TestMethod]
        public void OnTrackGamesShouldBeOnHold()
        {
            var gamesThatShouldBeOnHold = _tournament.Games
                .Where(g => g.IsCompleted() && g.Level == 2);

            gamesThatShouldBeOnHold.Should().BeEquivalentTo(_tournament.OnHoldGames);
        }

        [TestMethod]
        public void GameFromLaggingLevelShouldBeTheOnlyGameInProgress()
        {
            var gamesInProgress = _tournament.Games
                .Where(g => g.IsInProgress());

            gamesInProgress.SingleOrDefault().Should().Be(_NotPlayedGame);
        }

        [TestMethod]
        public void TournamentShouldNotBeCompleted()
        {
            _tournament.IsCompleted().Should().Be(false);
        }

        [TestMethod]
        public void SaveShouldHaveBeenCalled()
        {
            _MockRepository.Verify(_ => _.SaveChanges());
        }
    }
}
