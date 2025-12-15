using System;
using System.Collections.Generic;
using System.Linq;

namespace GameLibraryManager
{
    public class GameLibraryService : IGameLibraryService
    {
        private readonly List<Player> _players;
        private readonly List<Game> _games;
        private readonly List<PlayerGameStats> _stats;
        private readonly ILogger _logger;

        private readonly string _playersFile;
        private readonly string _gamesFile;
        private readonly string _statsFile;




        public GameLibraryService(ILogger logger)
        {
            _logger = logger;

            _playersFile = "players.json";
            _gamesFile = "games.json";
            _statsFile = "stats.json";

            _players = JsonStorage.LoadList<Player>(_playersFile);
            _games = JsonStorage.LoadList<Game>(_gamesFile);
            _stats = JsonStorage.LoadList<PlayerGameStats>(_statsFile);
        }


        public void AddPlayer(Player player)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));
            if (string.IsNullOrWhiteSpace(player.Username))
                throw new ArgumentException("Username is required.", nameof(player));

            _players.Add(player);
            JsonStorage.SaveList(_playersFile, _players);
            _logger.Log($"Added player {player.Username} (Id={player.Id}).");
        }

        public void AddGame(Game game)
        {
            if (game == null) throw new ArgumentNullException(nameof(game));
            if (string.IsNullOrWhiteSpace(game.Title))
                throw new ArgumentException("Title is required.", nameof(game));

            _games.Add(game);
            JsonStorage.SaveList(_gamesFile, _games);
            _logger.Log("Added game " + game.Title + " (Id=" + game.Id + ").");
        }

        public void RecordGameplay(int playerId, int gameId, double hours, int newScore)
        {
            if (hours <= 0) throw new ArgumentOutOfRangeException(nameof(hours));

            var player = FindPlayerById(playerId)
                         ?? throw new InvalidOperationException("Player not found.");
            var game = _games.Find(g => g.Id == gameId)
                       ?? throw new InvalidOperationException("Game not found.");

            var stats = _stats.Find(s => s.PlayerId == playerId && s.GameId == gameId);
            if (stats == null)
            {
                stats = new PlayerGameStats
                {
                    PlayerId = playerId,
                    GameId = gameId,
                    HoursPlayed = hours,
                    HighScore = newScore,
                    LastPlayed = DateTime.Now
                };
                _stats.Add(stats);
            }
            else
            {
                stats.HoursPlayed += hours; // BUG fixed :) : removed subtracting and added +=
                if (newScore > stats.HighScore)
                {
                    stats.HighScore = newScore;
                }
                stats.LastPlayed = DateTime.Now;
            }

            JsonStorage.SaveList(_statsFile, _stats);
            _logger.Log("Recorded gameplay for player " + player.Username + ".");
        }


        public Player FindPlayerById(int id)
        {
            if (playerId <= 0) throw new ArgumentOutOfRangeException(nameof(playerId));
            if (gameId <= 0) throw new ArgumentOutOfRangeException(nameof(gameId));
            // this will return null if not found :(
            return _players.FirstOrDefault(p => p.Id == id);
        }

        public Player FindPlayerByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username is required.", nameof(username)); // added throw exception

            return _players.FirstOrDefault(p =>
                p.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

        }

        public List<PlayerGameStats> GetTopPlayersByHours()
        {
            return _stats
                .OrderByDescending(s => s.HoursPlayed)
                .ToList();
        }
        public List<PlayerGameStats> GetTopPlayersByHoursManual()
        {
            // Make a copy so we do not change the original list
            List<PlayerGameStats> sorted = new List<PlayerGameStats>(_stats);

            int n = sorted.Count;
            bool swapped;

            // Simple bubble sort (descending by HoursPlayed)
            do
            {
                swapped = false;
                for (int i = 0; i < n - 1; i++)
                {
                    if (sorted[i].HoursPlayed < sorted[i + 1].HoursPlayed)
                    {
                        var temp = sorted[i];
                        sorted[i] = sorted[i + 1];
                        sorted[i + 1] = temp;
                        swapped = true;
                    }
                }
                n--;
            } while (swapped);

            return sorted;
        }

        public GameLibraryService(ILogger logger,
                          string playersFile,
                          string gamesFile,
                          string statsFile)
        {
            _logger = logger;

            _playersFile = playersFile;
            _gamesFile = gamesFile;
            _statsFile = statsFile;

            _players = JsonStorage.LoadList<Player>(_playersFile);
            _games = JsonStorage.LoadList<Game>(_gamesFile);
            _stats = JsonStorage.LoadList<PlayerGameStats>(_statsFile);
        }


    }
}
