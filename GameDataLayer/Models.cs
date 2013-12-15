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
        }

        [Key()]
        public virtual Int32 Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual DateTime StartDate { get; set; }

        public virtual IList<Player> Players { get; protected set; }

        public virtual IList<Game> Games { get; protected set; }

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
    }

    public partial class Player
    {
        public Player(string name)
        {
            this.Name = name;
        }

        [Key()]
        public virtual Int32 Id { get; set; }

        public virtual string Name { get; set; }

        public virtual Tournament Tournament { get; set; }
    }

    public partial class Game
    {
        public Game()
            : this(new List<Player>()) { }

        public Game(IEnumerable<Player> players)
        {
            this.Players = players.ToList();
        }

        [Key()]
        public virtual Int32 Id { get; set; }

        public virtual Player Winner { get; set; }

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
