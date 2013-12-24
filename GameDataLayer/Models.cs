using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GameDataLayer
{
    public partial class Tournament
    {
        public Tournament()
        {
            this.Players = new List<Player>();
            this.Games = new List<Game>();
            this.OnHoldGames = new List<Game>();
        }

        public Tournament(
            string name,
            string description,
            DateTime startDate,
            IList<Player> players,
            IList<Game> games)
        {
            this.Name = name;
            this.Description = description;
            this.StartDate = startDate;
            this.Players = players ?? new List<Player>();
            this.Games = games ?? new List<Game>();
            this.OnHoldGames = new List<Game>();
        }

        [Key()]
        public virtual Int32 TournamentId { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual DateTime StartDate { get; set; }

        public virtual IList<Player> Players { get; protected set; }

        public virtual IList<Game> Games { get; protected set; }

        public virtual IList<Game> OnHoldGames { get; protected set; }

        public Int32 WinnerId { get; set; }
        public virtual Player Winner { get; set; }

        public Player AddPlayer(Player player)
        {
            Players.Add(player);
            return player;
        }

        public Game AddGame(Game game)
        {
            Games.Add(game);
            game.Tournament = this;
            return game;
        }

        public bool IsCompleted()
        {
            return Winner != null;
        }

        public Game AddOnHoldGame(Game wonGame)
        {
            OnHoldGames.Add(wonGame);
            return wonGame;
        }

        public bool IsOnHold()
        {
            return OnHoldGames.Any();
        }

        public int CurrentLevel()
        {
            var highestLevelGame = Games.OrderByDescending(g => g.Level).FirstOrDefault();
            return highestLevelGame != null? highestLevelGame.Level : 0;
        }

        public void MarkAsResumed()
        {
            this.OnHoldGames.Clear();
        }
    }

    public partial class Player
    {
        public Player(string name)
        {
            this.Name = name;
        }

        [Key()]
        public virtual Int32 PlayerId { get; set; }

        public virtual string Name { get; set; }

        public Int32 TournamentId { get; set; }
        public virtual Tournament Tournament { get; set; }
    }

    public partial class Game
    {
        public Game()
            : this(1) { }

        public Game(int level)
            : this(level, new List<Player>()) { }

        public Game(int level, IEnumerable<Player> players)
        {
            this.Level = level;
            this.Players = players.ToList();
        }

        [Key()]
        public virtual Int32 GameId { get; set; }

        public int Level { get; set; }


        public Int32 WinnerId { get; set; }
        public virtual Player Winner { get; set; }


        public Int32 TournamentId { get; set; }
        public virtual Tournament Tournament { get; set; }

        public virtual IList<Player> Players { get; protected set; }

        public bool IsOpen()
        {
            return Players.Count == 1;
        }

        public bool IsInProgress()
        {
            return !IsOpen() && !IsCompleted();
        }

        public bool IsCompleted()
        {
            return Winner != null;
        }

        public Player AddPlayer(Player player)
        {
            Players.Add(player);
            return player;
        }
    }
}
