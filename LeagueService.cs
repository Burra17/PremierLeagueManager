using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace PremierLeagueManager
{
    public class LeagueService
    {
        private readonly DataStore<Team> teamStore;
        private readonly DataStore<Player> playerStore;
        private readonly string teamFile;
        private readonly string playerFile;

        public LeagueService(DataStore<Team> teamStore, DataStore<Player> playerStore, string teamFile, string playerFile)
        {
            this.teamStore = teamStore;
            this.playerStore = playerStore;
            this.teamFile = teamFile;
            this.playerFile = playerFile;
        }

        // === 💾 Spara/Ladda ===
        public void SaveAll()
        {
            teamStore.SaveToJson(teamFile);
            playerStore.SaveToJson(playerFile);
            MenuHelper.ShowSuccess("League data saved!");
        }

        public void LoadAll()
        {
            teamStore.LoadFromJson(teamFile);
            playerStore.LoadFromJson(playerFile);
        }

        // === ➕ Lägg till lag ===
        public void AddTeam()
        {
            MenuHelper.ShowSectionTitle("➕ Add New Team");
            string name = AnsiConsole.Ask<string>("Enter team name:");
            string manager = AnsiConsole.Ask<string>("Enter manager name:");

            var team = new Team(name, manager);
            teamStore.AddItem(team);
            MenuHelper.ShowSuccess($"Team '{name}' added!");
        }

        // === 👟 Lägg till spelare ===
        public void AddPlayer()
        {
            MenuHelper.ShowSectionTitle("👟 Add New Player");
            string name = AnsiConsole.Ask<string>("Enter player name:");
            string position = AnsiConsole.Ask<string>("Enter position:");
            string team = AnsiConsole.Ask<string>("Enter team name:");
            int goals = AnsiConsole.Ask<int>("Goals scored:");
            int assists = AnsiConsole.Ask<int>("Assists:");
            int rating = AnsiConsole.Ask<int>("Rating:");

            var player = new Player(name, position, goals, assists, team, rating);
            playerStore.AddItem(player);
            MenuHelper.ShowSuccess($"Player '{name}' added to {team}!");
        }

        // === 📊 Visa ligatabell ===
        public void ShowLeagueTable()
        {
            MenuHelper.ShowSectionTitle("📊 Premier League Table");

            var teams = teamStore.GetAllItems()
                .OrderByDescending(t => t.Points)
                .ThenByDescending(t => t.GoalsFor)
                .ToList();

            if (teams.Count == 0)
            {
                MenuHelper.ShowError("No teams in the league yet!");
                return;
            }

            var table = new Table()
                .AddColumn("Team")
                .AddColumn("Manager")
                .AddColumn("Points")
                .AddColumn("Goals For")
                .AddColumn("Goals Against");

            foreach (var team in teams)
            {
                table.AddRow(team.Name, team.Manager, team.Points.ToString(), team.GoalsFor.ToString(), team.GoalsAgainst.ToString());
            }

            AnsiConsole.Write(table);
        }

        // === 🏆 Visa toppskyttar ===
        public void ShowTopScorers()
        {
            MenuHelper.ShowSectionTitle("🏆 Top Scorers");

            var players = playerStore.GetAllItems()
                .OrderByDescending(p => p.GoalsScored)
                .ThenByDescending(p => p.Assists)
                .Take(10)
                .ToList();

            if (players.Count == 0)
            {
                MenuHelper.ShowError("No players found!");
                return;
            }

            var table = new Table()
                .AddColumn("Player")
                .AddColumn("Team")
                .AddColumn("Goals")
                .AddColumn("Assists")
                .AddColumn("Rating");

            foreach (var p in players)
            {
                table.AddRow(p.Name, p.Team, p.GoalsScored.ToString(), p.Assists.ToString(), p.Rating.ToString());
            }

            AnsiConsole.Write(table);
        }

        // === 🤖 Lägg till AI-genererat lag ===
        public void AddTeamFromAI(Team generatedTeam)
        {
            teamStore.AddItem(generatedTeam);
            foreach (var p in generatedTeam.Players)
                playerStore.AddItem(p);

            MenuHelper.ShowSuccess($"🤖 Team '{generatedTeam.Name}' generated with {generatedTeam.Players.Count} players!");
        }

        // === 🧹 Reset League Data ===
        public void ResetLeague()
        {
            try
            {
                // 🧠 Bekräftelse via MenuHelper
                if (!MenuHelper.ConfirmReset())
                {
                    MenuHelper.ShowError("Reset cancelled.");
                    return;
                }

                int teamsBefore = teamStore.GetAllItems().Count;
                int playersBefore = playerStore.GetAllItems().Count;

                // 🧹 Töm listor
                teamStore.GetAllItems().Clear();
                playerStore.GetAllItems().Clear();

                // 🗑️ Radera filer om de finns
                if (File.Exists(teamFile)) File.Delete(teamFile);
                if (File.Exists(playerFile)) File.Delete(playerFile);

                // 🔄 Återskapa tomma JSON-filer
                Directory.CreateDirectory(Path.GetDirectoryName(teamFile)!);
                File.WriteAllText(teamFile, "[]");
                File.WriteAllText(playerFile, "[]");

                // 🎬 Progress-animation
                AnsiConsole.Status()
                    .Spinner(Spinner.Known.Dots)
                    .SpinnerStyle(new Style(Color.Red))
                    .Start("Resetting league data...", ctx =>
                    {
                        Thread.Sleep(1000);
                    });

                MenuHelper.ShowSuccess($"All league data has been reset! ({teamsBefore} teams & {playersBefore} players deleted)");
            }
            catch (Exception ex)
            {
                MenuHelper.ShowError($"Error resetting data: {ex.Message}");
            }
        }
    }
}
