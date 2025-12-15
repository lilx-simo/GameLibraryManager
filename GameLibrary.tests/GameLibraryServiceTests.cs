using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameLibraryManager;
using System.IO;

namespace GameLibrary.Tests
{
    public class FakeLogger : ILogger
    {
        public void Log(string message)
        {
            // do nothing in tests
        }
    }

    [TestClass]
    public class GameLibraryServiceTests
    {
        private GameLibraryService CreateService()
        {
            // Use test-specific files so counts start from zero
            var service = new GameLibraryService(
                new FakeLogger(),
                "players_test.json",
                "games_test.json",
                "stats_test.json");

            return service;
        }


        [TestMethod]
        public void AddPlayer_ValidPlayer_IsAdded()
        {
            var service = CreateService();
            var player = new Player { Id = 1, Username = "test", Email = "t@test.com" };

            service.AddPlayer(player);
            var found = service.FindPlayerById(1);

            Assert.IsNotNull(found);
            Assert.AreEqual("test", found.Username);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddPlayer_EmptyUsername_Throws()
        {
            var service = CreateService();
            var player = new Player { Id = 2, Username = "", Email = "x@test.com" };

            service.AddPlayer(player);
        }

        [TestMethod]
        public void RecordGameplay_FirstTime_CreatesStats()
        {
            var service = CreateService();

            var player = new Player { Id = 1, Username = "p1", Email = "p1@test.com" };
            var game = new Game { Id = 10, Title = "Game A", Genre = "Action" };
            service.AddPlayer(player);
            service.AddGame(game);

            service.RecordGameplay(1, 10, 2.5, 100);

            var stats = service.GetTopPlayersByHoursManual();
            Assert.AreEqual(1, stats.Count);
            Assert.AreEqual(1, stats[0].PlayerId);
            Assert.AreEqual(10, stats[0].GameId);
            Assert.AreEqual(2.5, stats[0].HoursPlayed, 0.001);
            Assert.AreEqual(100, stats[0].HighScore);
        }

        [TestMethod]
        public void RecordGameplay_SecondSession_UpdatesHoursAndHighScore()
        {
            var service = CreateService();

            var player = new Player { Id = 1, Username = "p1", Email = "p1@test.com" };
            var game = new Game { Id = 10, Title = "Game A", Genre = "Action" };
            service.AddPlayer(player);
            service.AddGame(game);

            service.RecordGameplay(1, 10, 1.0, 50);
            service.RecordGameplay(1, 10, 2.0, 120);

            var stats = service.GetTopPlayersByHoursManual();
            Assert.AreEqual(1, stats.Count);
            Assert.AreEqual(3.0, stats[0].HoursPlayed, 0.001);   // 1 + 2
            Assert.AreEqual(120, stats[0].HighScore);            // updated to higher score
        }

        [TestMethod]
        public void GetTopPlayersByHoursManual_SortsDescending()
        {
            var service = CreateService();

            var p1 = new Player { Id = 1, Username = "p1", Email = "p1@test.com" };
            var p2 = new Player { Id = 2, Username = "p2", Email = "p2@test.com" };
            var game = new Game { Id = 10, Title = "Game A", Genre = "Action" };
            service.AddPlayer(p1);
            service.AddPlayer(p2);
            service.AddGame(game);

            service.RecordGameplay(1, 10, 1.0, 50);  // 1 hour
            service.RecordGameplay(2, 10, 5.0, 60);  // 5 hours

            var stats = service.GetTopPlayersByHoursManual();

            Assert.AreEqual(2, stats.Count);
            Assert.IsTrue(stats[0].HoursPlayed >= stats[1].HoursPlayed);
            Assert.AreEqual(2, stats[0].PlayerId);   // player 2 should come first
        }




        [TestInitialize]
        public void Setup()
        {
            File.Delete("players_test.json");
            File.Delete("games_test.json");
            File.Delete("stats_test.json");
        }


    }
}



