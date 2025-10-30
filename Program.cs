using Spectre.Console;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace PremierLeagueManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // === Filvägar ===
            string dataDir = "data";
            string teamFile = Path.Combine(dataDir, "teams.json");
            string playerFile = Path.Combine(dataDir, "players.json");

            if (!Directory.Exists(dataDir))
                Directory.CreateDirectory(dataDir);

            // === Initiera datalagring ===
            var teamStore = new DataStore<Team>();
            var playerStore = new DataStore<Player>();

            // === Ladda befintlig data ===
            AnsiConsole.Status()
                .Spinner(Spinner.Known.BouncingBar)
                .Start("Loading data...", ctx =>
                {
                    teamStore.LoadFromJson(teamFile);
                    playerStore.LoadFromJson(playerFile);
                    Thread.Sleep(800);
                });

            bool running = true;

            while (running)
            {
                string choice = MenuHelper.ShowMainMenu();

                switch (choice)
                {
                    // === LÄGG TILL LAG ===
                    case "➕ Add Team":
                        MenuHelper.ShowSectionTitle("Add a new Team");

                        string teamName = AnsiConsole.Ask<string>("Team name:");
                        string managerName = AnsiConsole.Ask<string>("Manager name:");

                        var newTeam = new Team(teamName, managerName);
                        teamStore.AddItem(newTeam);

                        MenuHelper.ShowSuccess($"Team '{teamName}' added successfully!");
                        break;

                    // === VISA TABELL ===
                    case "📊 Show Team Table":
                        ShowLeagueTable(teamStore);
                        break;

                    // === LÄGG TILL SPELARE ===
                    case "👟 Add Player":
                        MenuHelper.ShowSectionTitle("Add a new Player");

                        string playerName = AnsiConsole.Ask<string>("Player name:");
                        string position = AnsiConsole.Ask<string>("Position:");
                        int goals = AnsiConsole.Ask<int>("Goals scored:");
                        int assists = AnsiConsole.Ask<int>("Assists:");
                        string playerTeam = AnsiConsole.Ask<string>("Team name:");
                        int rating = AnsiConsole.Ask<int>("Rating (1-100):");

                        var newPlayer = new Player(playerName, position, goals, assists, playerTeam, rating);
                        playerStore.AddItem(newPlayer);

                        // Koppla till lag (om det finns)
                        var team = teamStore.Items.FirstOrDefault(t => t.Name.Equals(playerTeam, StringComparison.OrdinalIgnoreCase));
                        if (team != null)
                            team.Players.Add(newPlayer);

                        MenuHelper.ShowSuccess($"Player '{playerName}' added to '{playerTeam}'!");
                        break;

                    // === TOP SCORERS ===
                    case "🏆 Show Top Scorers":
                        ShowTopScorers(playerStore);
                        break;

                    // === SPARA & AVSLUTA ===
                    case "💾 Save and Exit":
                        MenuHelper.ShowSaveAnimation();
                        teamStore.SaveToJson(teamFile);
                        playerStore.SaveToJson(playerFile);
                        running = false;
                        break;
                }

                if (running)
                {
                    AnsiConsole.MarkupLine("\n[yellow]Press any key to return to the menu...[/]");
                    Console.ReadKey(true);
                }
            }
        }

        // === VISA LIGATABELL ===
        static void ShowLeagueTable(DataStore<Team> teamStore)
        {
            MenuHelper.ShowSectionTitle("🏟️ Premier League Table");

            if (!teamStore.Items.Any())
            {
                MenuHelper.ShowError("No teams available.");
                return;
            }

            var sortedTeams = teamStore.Items
                .OrderByDescending(t => t.Points)
                .ThenByDescending(t => t.GoalDifference)
                .ToList();

            var table = new Table().BorderColor(Color.Grey);
            table.AddColumn("Pos");
            table.AddColumn("Team");
            table.AddColumn("Manager");
            table.AddColumn("Pts");
            table.AddColumn("GD");

            int rank = 1;
            foreach (var t in sortedTeams)
            {
                string color = rank switch
                {
                    <= 4 => "green",      // Top 4
                    >= 18 => "red",       // Relegation
                    _ => "yellow"         // Mid-table
                };

                table.AddRow(
                    $"[{color}]{rank}[/]",
                    $"[{color}]{t.Name}[/]",
                    t.Manager,
                    t.Points.ToString(),
                    t.GoalDifference.ToString()
                );

                rank++;
            }

            AnsiConsole.Write(table);
        }

        // === VISA TOP 5 MÅLSKYTTAR ===
        static void ShowTopScorers(DataStore<Player> playerStore)
        {
            MenuHelper.ShowSectionTitle("🏆 Top Scorers");

            if (!playerStore.Items.Any())
            {
                MenuHelper.ShowError("No players available.");
                return;
            }

            var topPlayers = playerStore.Items
                .OrderByDescending(p => p.GoalsScored)
                .ThenByDescending(p => p.Assists)
                .Take(5)
                .ToList();

            var table = new Table().BorderColor(Color.Grey);
            table.AddColumn("Rank");
            table.AddColumn("Name");
            table.AddColumn("Team");
            table.AddColumn("Goals");
            table.AddColumn("Assists");
            table.AddColumn("⭐ Rating");

            int rank = 1;
            foreach (var p in topPlayers)
            {
                var starColor = p.Rating >= 85 ? "green" : p.Rating >= 70 ? "yellow" : "red";
                table.AddRow(
                    $"[bold]{rank}[/]",
                    p.Name,
                    p.Team,
                    p.GoalsScored.ToString(),
                    p.Assists.ToString(),
                    $"[{starColor}]{p.Rating}[/]"
                );
                rank++;
            }

            AnsiConsole.Write(table);
        }
    }
}
