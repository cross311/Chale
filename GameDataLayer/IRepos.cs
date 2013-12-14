using System;
using System.Collections.Generic;

namespace GameDataLayer
{
    public interface ITournamentRepo
    {
        Tournament Get(Guid id);

        IList<Tournament> Get();

        Tournament Save(Tournament tournament);

        Tournament AddPlayer(Tournament tournament, Player player);

        Tournament AddGame(Tournament tournament, Game game);
    }

    public interface IGameRepo
    {
        Game Get(Guid id);

        IList<Game> GetByTournament(Guid tournamentId);

        Game Save(Game game);

        Game AddPlayer(Game game, Player player);

        Game AddPlayers(Game game, IEnumerable<Player> players);
    }

    public interface IPlayerRepo
    {
        Player Get(Guid id);

        IList<Player> GetByTournament(Guid tournamentId);

        IList<Player> GetByGame(Guid gameId);

        Player Save(Player player);
    }
}
