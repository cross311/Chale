using System;
using System.Collections.Generic;

namespace GameDataLayer
{
    public interface ITournamentRepo
    {
        Tournament Get(Guid id);

        IList<Tournament> Get();
    }

    public interface IGameRepo
    {
        Game Get(Guid id);

        IList<Game> GetByTournament(Guid tournamentId);
    }

    public interface IPlayerRepo
    {
        Player Get(Guid id);

        IList<Player> GetByTournament(Guid tournamentId);

        IList<Player> GetByGame(Guid gameId);
    }
}
