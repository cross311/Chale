using System;

namespace GameDataLayer
{
    public partial class Tournament
    {
        public virtual Guid Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual DateTime? StartDate { get; set; }
    }

    public partial class Player
    {
        public virtual Guid Id { get; set; }

        public virtual string Name { get; set; }

        public virtual Guid TournamentId { get; set; }
    }

    public partial class Game
    {
        public virtual Guid Id { get; set; }

        public virtual Guid WinnerId { get; set; }

        public virtual Guid TournamentId { get; set; }
    }
}
