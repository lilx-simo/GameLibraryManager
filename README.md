# Game Library & Player Stats Manager

## Overview
A C# console application demonstrating OOP principles, unit testing, and design patterns. The system manages a game library and tracks player gameplay statistics with persistence and logging capabilities.

## Features
- **Player Management**: Add players with username and email validation
- **Game Catalog**: Maintain a catalog of games by title and genre
- **Gameplay Recording**: Log player sessions with hours played and high scores
- **Player Search**: Find players by ID or username using linear search algorithm
- **Statistics Ranking**: View top players by hours played using bubble sort algorithm
- **Data Persistence**: Save and load data using JSON serialization
- **Audit Logging**: All operations logged to a file for traceability
- **Unit Testing**: 6 comprehensive unit tests validating core functionality

## Technologies & Patterns
- **Language**: C# (.NET Framework)
- **Data Storage**: JSON (Newtonsoft.Json/Json.NET)
- **Testing**: MSTest Unit Test Framework
- **Design Patterns**: 
  - Dependency Injection (GameLibraryService depends on ILogger)
  - Repository Pattern (JsonStorage for data access)
  - Strategy Pattern (ILogger interface with FileLogger implementation)
  - Singleton-like usage (Static JsonStorage utility)
- **Version Control**: Git/GitHub

## Project Structure

GameLibraryManager/
├── ConsoleApp1/
│ ├── Program.cs # Main entry point with menu system
│ ├── Player.cs # Player entity
│ ├── Game.cs # Game entity
│ ├── PlayerGameStats.cs # Gameplay statistics entity
│ ├── IGameLibraryService.cs # Service interface
│ ├── GameLibraryService.cs # Core business logic & data management
│ ├── ILogger.cs # Logging interface
│ ├── FileLogger.cs # File-based logger implementation
│ ├── JsonStorage.cs # Static JSON persistence utility
│ └── GameLibraryManager.csproj
├── GameLibrary.Tests/
│ ├── GameLibraryServiceTests.cs # Unit tests (6 tests)
│ └── GameLibrary.Tests.csproj
├── GameLibraryManager.sln
└── README.md


## How to Run

### Prerequisites
- Visual Studio 2019 or later
- .NET Framework 4.7.2+
- Newtonsoft.Json NuGet package

### Steps
1. Clone the repository:
git clone https://github.com/lilx-simo/GameLibraryManager.git

2. Open `GameLibraryManager.sln` in Visual Studio

3. Restore NuGet packages:
- Right-click solution → "Restore NuGet Packages"
- Or: `Tools → NuGet Package Manager → Manage NuGet Packages for Solution`

4. Build the solution:
- `Build → Build Solution` (Ctrl+Shift+B)

5. Run the console application:
- Select `ConsoleApp1` as startup project
- Press F5 or `Debug → Start Debugging`

6. Use the menu to interact with the application

### Running Unit Tests

1. Open **Test Explorer**:
- `Test → Test Explorer` (Ctrl+E, T)

2. Click **Run All Tests**

3. Verify all 6 tests pass (green checkmarks)

## Unit Tests

The test suite includes 6 comprehensive tests:

| Test Name | Purpose |
|-----------|---------|
| `AddPlayer_ValidPlayer_AddsSuccessfully` | Validates player addition |
| `AddGame_ValidGame_AddsSuccessfully` | Validates game addition |
| `RecordGameplay_FirstSession_CreatesNewStats` | Tests initial gameplay recording |
| `RecordGameplay_SecondSession_UpdatesHoursAndHighScore` | Tests stats accumulation |
| `FindPlayerById_ExistingPlayer_ReturnsPlayer` | Validates player search by ID |
| `GetTopPlayersByHoursManual_SortsDescending` | Tests sorting algorithm |

**Running Tests**:
From Visual Studio Test Explorer
Right-click test → Run Selected Tests

Or run all
Test → Run All Tests


## Design Decisions

### Dependency Injection
`GameLibraryService` accepts an `ILogger` through its constructor, allowing different logger implementations without coupling to a specific logger class.

### Repository Pattern
`JsonStorage` encapsulates all file I/O operations, providing a clean separation between data persistence and business logic.

### Algorithms
- **Linear Search**: `FindPlayerById()` and `FindPlayerByUsername()` use simple iteration
- **Bubble Sort**: `GetTopPlayersByHoursManual()` implements bubble sort to rank players by hours

### Error Handling
- Graceful handling of missing or corrupted JSON files
- Input validation for player/game addition
- Null checks for search operations

## Data Files

The application creates the following JSON files in the working directory:

- `players.json` - Serialized player objects
- `games.json` - Serialized game objects
- `stats.json` - Serialized player-game statistics
- `logs.txt` - Audit log of all operations

Example `players.json`:
							[
							{
							"Id": 1,
							"Username": "player1",
							"Email": "player1@gmail.com"
							}
							]

## Debugging Example

The project includes a deliberate bug fix scenario (Task 3: Testing):
1. Bug: Subtraction operator (`-=`) in gameplay hours recording
2. Detection: Unit test failure with assertion error
3. Debugging: Breakpoints and Watch window analysis
4. Resolution: Operator corrected to addition (`+=`)

This demonstrates practical debugging workflows using Visual Studio's debugging tools.

## Future Enhancements

- Add user authentication
- Implement advanced search filters
- Add leaderboard visualization
- Support multiple file formats (CSV, XML)
- Create a WPF/WinForms GUI
- Add achievement system
- Implement cloud-based data sync

## Author
Simo  
December 2025

## License
Moi