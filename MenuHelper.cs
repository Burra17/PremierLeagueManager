using Spectre.Console;
using System;
using System.Threading;

namespace PremierLeagueManager
{
    public static class MenuHelper
    {
        public static string ShowMainMenu()
        {
            Console.Clear();

            // Titel i stor, snygg text
            AnsiConsole.Write(
                new FigletText("Premier League Manager")
                    .Centered()
                    .Color(Color.Red));

            AnsiConsole.WriteLine();

            // Valmeny med färger och emojis
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]⚽ Välj ett alternativ:[/]")
                    .HighlightStyle(new Style(foreground: Color.Green, decoration: Decoration.Bold))
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "➕ Add Team",
                        "📊 Show Team Table",
                        "👟 Add Player",
                        "🏆 Show Top Scorers",
                        "💾 Save and Exit"
                    }));

            return choice;
        }

        // === Hjälpmetoder ===
        public static void ShowSectionTitle(string title)
        {
            AnsiConsole.MarkupLine($"\n[bold cyan]{title}[/]\n");
        }

        public static void ShowSuccess(string message)
        {
            AnsiConsole.MarkupLine($"[green]✔ {message}[/]");
        }

        public static void ShowError(string message)
        {
            AnsiConsole.MarkupLine($"[red]❌ {message}[/]");
        }

        // === 🔥 Snygg spar-animation utan error ===
        public static void ShowSaveAnimation()
        {
            Console.Clear();
            AnsiConsole.Write(
                new FigletText("Saving Data...")
                    .Centered()
                    .Color(Color.Yellow));

            Thread.Sleep(800);

            // Spinner med textfaser
            AnsiConsole.Status()
                .Spinner(Spinner.Known.BouncingBar)
                .SpinnerStyle(Style.Parse("green"))
                .Start("Preparing data...", ctx =>
                {
                    Thread.Sleep(800);
                    ctx.Status("Writing files...");
                    Thread.Sleep(1000);
                    ctx.Status("Finalizing...");
                    Thread.Sleep(800);
                });

            // Progressbar separat (för att undvika Spectre-error)
            AnsiConsole.WriteLine();
            AnsiConsole.Progress()
                .HideCompleted(false)
                .Columns(new ProgressColumn[]
                {
                    new TaskDescriptionColumn(),
                    new ProgressBarColumn(),
                    new PercentageColumn(),
                    new RemainingTimeColumn()
                })
                .Start(ctx =>
                {
                    var task = ctx.AddTask("[yellow]Saving League Data[/]");
                    while (!task.IsFinished)
                    {
                        task.Increment(5);
                        Thread.Sleep(70);
                    }
                });

            // Snyggt avslut med färg och animation
            AnsiConsole.MarkupLine("\n[bold green]✔ All data saved successfully![/]");
            AnsiConsole.MarkupLine("[grey]Cleaning up temporary files...[/]");
            Thread.Sleep(600);
            AnsiConsole.MarkupLine("[green]✨ Done![/]");
            Thread.Sleep(600);

            // Liten avslutande text med gradient
            AnsiConsole.Write(
                new Panel(new Markup("[bold yellow]Goodbye, Manager![/]\n[grey]See you next matchday ⚽[/]"))
                    .Border(BoxBorder.Rounded)
                    .BorderColor(Color.Green)
                    .Header("[bold red]Session Closed[/]")
                    .HeaderAlignment(Justify.Center)
                    .Expand());
        }
    }
}
