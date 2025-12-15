using System;
using System.Collections.Generic;

namespace GameLibraryManager
{
    internal class Program
    {
        static void Main()
        {
            var logger = new FileLogger("logs.txt");
            var service = new GameLibraryService(logger);

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("=== Game Library & Player Stats ===");
                Console.WriteLine("1. Add player");
                Console.WriteLine("2. Add game");
                Console.WriteLine("3. Record gameplay");
                Console.WriteLine("4. Find player by ID");
                Console.WriteLine("5. Find player by username");
                Console.WriteLine("6. Show top players by hours");
                Console.WriteLine("0. Exit");
                Console.Write("Choice: ");

                var input = Console.ReadLine();
                Console.WriteLine();

                switch (input)
                {
                    case "1":
                        AddPlayerFlow(service);
                        break;
                    case "2":
                        AddGameFlow(service);
                        break;
                    case "3":
                        RecordGameplayFlow(service);
                        break;
                    case "4":
                        FindPlayerByIdFlow(service);
                        break;
                    case "5":
                        FindPlayerByUsernameFlow(service);
                        break;
                    case "6":
                        ShowTopPlayersFlow(service);
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

                Console.WriteLine();
            }
        }

        static void AddPlayerFlow(IGameLibraryService service)
        {
            Console.Write("Player Id: ");
            int id = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Username: ");
            string username = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            var player = new Player { Id = id, Username = username ?? "", Email = email ?? "" };
            service.AddPlayer(player);
            Console.WriteLine("Player added.");
        }

        static void AddGameFlow(IGameLibraryService service)
        {
            Console.Write("Game Id: ");
            int id = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Title: ");
            string title = Console.ReadLine();

            Console.Write("Genre: ");
            string genre = Console.ReadLine();

            var game = new Game { Id = id, Title = title ?? "", Genre = genre ?? "" };
            service.AddGame(game);
            Console.WriteLine("Game added.");
        }

        static void RecordGameplayFlow(IGameLibraryService service)
        {
            Console.Write("Player Id: ");
            int playerId = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Game Id: ");
            int gameId = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Hours played: ");
            double hours = double.Parse(Console.ReadLine() ?? "0");

            Console.Write("New score (0 if none): ");
            int score = int.Parse(Console.ReadLine() ?? "0");

            service.RecordGameplay(playerId, gameId, hours, score);
            Console.WriteLine("Gameplay recorded.");
        }

        static void FindPlayerByIdFlow(IGameLibraryService service)
        {
            Console.Write("Player Id: ");
            int id = int.Parse(Console.ReadLine() ?? "0");
            var player = service.FindPlayerById(id);
            Console.WriteLine(player == null
                ? "Player not found."
                : $"Id={player.Id}, Username={player.Username}, Email={player.Email}");
        }

        static void FindPlayerByUsernameFlow(IGameLibraryService service)
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();
            var player = username == null ? null : service.FindPlayerByUsername(username);
            Console.WriteLine(player == null
                ? "Player not found."
                : $"Id={player.Id}, Username={player.Username}, Email={player.Email}");
        }

        static void ShowTopPlayersFlow(IGameLibraryService service)
        {
            var stats = service.GetTopPlayersByHoursManual();
            if (stats.Count == 0)
            {
                Console.WriteLine("No stats yet.");
                return;
            }

            foreach (var s in stats)
            {
                Console.WriteLine("PlayerId=" + s.PlayerId +
                                  ", GameId=" + s.GameId +
                                  ", Hours=" + s.HoursPlayed +
                                  ", HighScore=" + s.HighScore);
            }
        }

    }
}


