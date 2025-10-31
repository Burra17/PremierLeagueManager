using Spectre.Console;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PremierLeagueManager
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            string dataDir = "data";
            Directory.CreateDirectory(dataDir);

            string teamFile = Path.Combine(dataDir, "teams.json");
            string playerFile = Path.Combine(dataDir, "players.json");

            var teamStore = new DataStore<Team>();
            var playerStore = new DataStore<Player>();
            var leagueService = new LeagueService(teamStore, playerStore, teamFile, playerFile);
            var aiService = new AiAssistanceService();

            // === 🔄 Startanimation ===
            AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .SpinnerStyle(Style.Parse("cyan"))
                .Start("Initializing Premier League database...", ctx =>
                {
                    System.Threading.Thread.Sleep(1500);
                });

            // === 📂 Ladda data ===
            AnsiConsole.Status()
                .Spinner(Spinner.Known.BouncingBar)
                .SpinnerStyle(Style.Parse("green"))
                .Start("Loading data...", ctx =>
                {
                    leagueService.LoadAll();
                    System.Threading.Thread.Sleep(800);
                });

            bool running = true;
            while (running)
            {
                string choice = MenuHelper.ShowMainMenu();

                switch (choice)
                {
                    case "➕ Add Team":
                        leagueService.AddTeam();
                        break;

                    case "👟 Add Player":
                        leagueService.AddPlayer();
                        break;

                    case "📊 Show Team Table":
                        leagueService.ShowLeagueTable();
                        break;

                    case "🏆 Show Top Scorers":
                        leagueService.ShowTopScorers();
                        break;

                    case "🤖 Generate Team (AI)":
                        await aiService.GenerateTeamInteractiveAsync(leagueService);
                        break;

                    case "🧹 Reset League Data":
                        leagueService.ResetLeague();
                        break;

                    case "💾 Save and Exit":
                        leagueService.SaveAll();
                        MenuHelper.ShowSaveAnimation(); // 🧩 visa snygg animation innan exit
                        running = false;
                        break;
                }

                if (running)
                {
                    AnsiConsole.MarkupLine("\n[yellow]Press any key to return to the menu...[/]");
                    Console.ReadKey(true);
                }
            }

            // 🕹️ Visa avslutande meddelande så du hinner se det innan fönstret stänger
            AnsiConsole.MarkupLine("\n[grey]Press any key to close the program...[/]");
            Console.ReadKey(true);
        }
    }
}
