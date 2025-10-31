using Spectre.Console;
using System;
using System.Threading;

namespace PremierLeagueManager
{
    public static class MenuHelper
    {
        // === ⚽ Huvudmeny ===
        public static string ShowMainMenu()
        {
            Console.Clear();

            // Titel
            AnsiConsole.Write(
                new FigletText("Premier League Manager")
                    .Centered()
                    .Color(Color.Red));

            AnsiConsole.MarkupLine("[yellow]Welcome, Manager![/]\n");

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]⚽ Main Menu[/]")
                    .HighlightStyle(new Style(Color.Green, decoration: Decoration.Bold))
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "➕ Add Team",
                        "👟 Add Player",
                        "📊 Show Team Table",
                        "🏆 Show Top Scorers",
                        "🤖 Generate Team (AI)",
                        "🧹 Reset League Data",
                        "💾 Save and Exit"
                    }));

            return choice;
        }

        // === 📘 Sektionstitel ===
        public static void ShowSectionTitle(string title)
        {
            var safeTitle = Markup.Escape(title);
            AnsiConsole.Write(new Rule($"[bold cyan]{safeTitle}[/]").LeftJustified());
            AnsiConsole.WriteLine();
        }

        // === ✅ Framgång ===
        public static void ShowSuccess(string message)
        {
            var safe = Markup.Escape(message);
            AnsiConsole.MarkupLine($"[green]✔ {safe}[/]");
        }

        // === ❌ Fel ===
        public static void ShowError(string message)
        {
            var safe = Markup.Escape(message);
            AnsiConsole.MarkupLine($"[red]❌ {safe}[/]");
        }

        // === 💾 Sparanimation ===
        public static void ShowSaveAnimation()
        {
            Console.Clear();

            AnsiConsole.Write(
                new FigletText("Saving Data...")
                    .Centered()
                    .Color(Color.Yellow));

            Thread.Sleep(500);

            // Status (utan Parse)
            AnsiConsole.Status()
                .Spinner(Spinner.Known.BouncingBar)
                .SpinnerStyle(new Style(Color.Green))
                .Start("Preparing data...", ctx =>
                {
                    Thread.Sleep(600);
                    ctx.Status("Writing files...");
                    Thread.Sleep(800);
                    ctx.Status("Finalizing...");
                    Thread.Sleep(500);
                });

            // Progressbar
            AnsiConsole.WriteLine();
            AnsiConsole.Progress()
                .HideCompleted(false)
                .Columns(
                    new TaskDescriptionColumn(),
                    new ProgressBarColumn(),
                    new PercentageColumn(),
                    new RemainingTimeColumn()
                )
                .Start(ctx =>
                {
                    var task = ctx.AddTask("[yellow]Saving League Data[/]");
                    while (!task.IsFinished)
                    {
                        task.Increment(5);
                        Thread.Sleep(60);
                    }
                });

            // Avslutande text
            AnsiConsole.MarkupLine("\n[bold green]✔ All data saved successfully![/]");
            Thread.Sleep(400);

            var panel = new Panel(new Markup("[bold yellow]Goodbye, Manager![/]\n[grey]See you next matchday ⚽[/]"))
                .Border(BoxBorder.Rounded)
                .BorderColor(Color.Green)
                .Header("[bold red]Session Closed[/]")
                .HeaderAlignment(Justify.Center)
                .Expand();

            AnsiConsole.Write(panel);
            Thread.Sleep(600);
        }

        // === 🔁 Bekräftelse för reset ===
        public static bool ConfirmReset()
        {
            return AnsiConsole.Confirm("[bold red]⚠️ Are you sure you want to reset all league data?[/]");
        }
    }
}
