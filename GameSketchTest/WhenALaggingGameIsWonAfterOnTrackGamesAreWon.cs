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
    public class WhenALaggingGameIsWonAfterOnTrackGamesAreWon
    {
        private Tournament _tournament;
        private TournamentService _tournamentService;
        private Mock<IRepository<Tournament>> _MockRepository;
        private Game _laggingGame;
        private Player _laggingGameWinner;

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


        public WhenALaggingGameIsWonAfterOnTrackGamesAreWon()
        {
            _tournament = new Tournament();
            _MockRepository = new Mock<IRepository<Tournament>>();
            _tournamentService = new TournamentService(_MockRepository.Object);

            for(int pIndex = 0; pIndex < _players.Count; pIndex++)
                _tournament.AddPlayer(_players[pIndex]);

            _tournamentService.Start(_tournament);
            _laggingGame = _tournament.Games[0];

            _tournament.Games
                .Where(g => g != _laggingGame)
                .ToList().ForEach(g => _tournamentService.GameWon(_tournament, g, g.Players[0]));

            _tournament.Games
                .Where(g => g != _laggingGame && g.IsInProgress())
                .ToList().ForEach(g => _tournamentService.GameWon(_tournament, g, g.Players[0]));

            _laggingGameWinner = _laggingGame.Players[0];
            _tournamentService.GameWon(_tournament, _laggingGame, _laggingGameWinner);
        }

        [TestMethod]
        public void NextLevelShouldBeInProgress()
        {
            var gamesInProgress = _tournament.Games
                .Where(g => g.IsInProgress());

            gamesInProgress.Count().Should().Be(2);
        }

        [TestMethod]
        public void AllGamesInProgressShouldBeAtSameLevel()
        {
            var gamesInProgress = _tournament.Games
                .Where(g => g.IsInProgress());

            gamesInProgress.All(g => g.Level == 3).Should().Be(true);
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
