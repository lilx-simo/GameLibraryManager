using System.Collections.Generic;

namespace GameLibraryManager
{
    public interface IGameLibraryService
    {
        void AddPlayer(Player player);
        void AddGame(Game game);
        void RecordGameplay(int playerId, int gameId, double hours, int newScore);
        Player FindPlayerById(int id);
        Player FindPlayerByUsername(string username);
        List<PlayerGameStats> GetTopPlayersByHours();
        List<PlayerGameStats> GetTopPlayersByHoursManual();

    }
}


