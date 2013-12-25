using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace GameDataLayer
{
    public partial class Tournament
    {
        public Tournament()
        {
            this.Players = new List<Player>();
            this.Games = new List<Game>();
        }

        //public Tournament(
        //    string name,
        //    string description,
        //    DateTime startDate,
        //    List<Player> players,
        //    List<Game> games)
        //{
        //    this.Name = name;
        //    this.Description = description;
        //    this.StartDate = startDate;
        //    this.Players = players ?? new List<Player>();
        //    this.Games = games ?? new List<Game>();
        //    this.OnHoldGames = new List<Game>();
        //}

        [Key()]
        public Int32 TournamentId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        [InverseProperty("Tournament")]
        public virtual List<Player> Players { get; protected set; }

        [InverseProperty("Tournament")]
        public virtual List<Game> Games { get; protected set; }

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

        public IEnumerable<Game> OnHoldGames
        {
            get
            {
                return Games.Where(game => game.OnHold);
            }
        }

        public Game AddOnHoldGame(Game wonGame)
        {
            wonGame.OnHold = true;
            return wonGame;
        }

        public bool IsOnHold()
        {
            return OnHoldGames.Any();
        }

        public int CurrentLevel()
        {
            var highestLevelGame = Games.OrderByDescending(g => g.Level).FirstOrDefault();
            return highestLevelGame != null ? highestLevelGame.Level : 0;
        }

        public void MarkAsResumed()
        {
            OnHoldGames
                .ToList()
                .ForEach(game => game.OnHold = false);
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

        public string Name { get; set; }

        [InverseProperty("Players")]
        public virtual Tournament Tournament { get; set; }

        [InverseProperty("Players")]
        public virtual List<Game> Games { get; set; }

        [InverseProperty("Winner")]
        public virtual List<Game> WonGames { get; set; }
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
        public Int32 GameId { get; set; }

        public int Level { get; set; }

        public bool OnHold { get; set; }

        [InverseProperty("WonGames")]
        public virtual Player Winner { get; set; }

        [InverseProperty("Games")]
        public virtual Tournament Tournament { get; set; }

        [InverseProperty("Games")]
        public virtual List<Player> Players { get; protected set; }

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
