using System;
using System.Collections.Generic;

namespace GameDataLayer
{
    public partial class Tournament
    {
        public Tournament()
        {
            this.Games = new List<Game>();
            this.Players = new List<Player>();
        }

        public virtual Guid Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual DateTime? StartDate { get; set; }

        public virtual Player Winner { get; set; }

        public virtual IList<Game> Games { get; protected set; }

        public virtual IList<Player> Players { get; protected set; }

        public bool IsCompleted()
        {
            return Winner != null;
        }
    }

    public partial class Player
    {
        public Player()
        {
            this.Games = new List<Game>();
        }

        public virtual Guid Id { get; set; }

        public virtual string Name { get; set; }

        public virtual Tournament Tournament { get; set; }

        public virtual IList<Game> Games { get; protected set; }
    }

    public partial class Game
    {
        public Game()
        {
            this.Players = new List<Player>();
        }

        public virtual Guid Id { get; set; }

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
    }
}
