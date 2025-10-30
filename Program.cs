using Spectre.Console;
using System;

namespace PremierLeagueManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // För att stödja emojis i konsolen

            bool running = true;

            while (running)
            {
                string choice = MenuHelper.ShowMainMenu();

                switch (choice)
                {
                    case "➕ Add Team":
                        MenuHelper.ShowSectionTitle("Add a new Team");
                        MenuHelper.ShowSuccess("Team added successfully!");
                        break;

                    case "📊 Show Team Table":
                        MenuHelper.ShowSectionTitle("Premier League Table");
                        MenuHelper.ShowSuccess("Displayed table!");
                        break;

                    case "👟 Add Player":
                        MenuHelper.ShowSectionTitle("Add a new Player");
                        MenuHelper.ShowSuccess("Player added!");
                        break;

                    case "🏆 Show Top Scorers":
                        MenuHelper.ShowSectionTitle("Top Scorers");
                        MenuHelper.ShowSuccess("Top scorers shown!");
                        break;

                    case "💾 Save and Exit":
                        MenuHelper.ShowSaveAnimation();
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
    }
}
