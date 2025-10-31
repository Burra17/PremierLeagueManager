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
            AnsiConsole.Write(
                new FigletText("Premier League Manager")
                    .Centered()
                    .Color(Color.Red));

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]⚽ Välj ett alternativ:[/]")
                    .HighlightStyle(new Style(foreground: Color.Green, decoration: Decoration.Bold))
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

        public static void ShowSectionTitle(string title)
        {
            AnsiConsole.MarkupLine($"\n[bold cyan]{title}[/]\n");
        }

        public static void ShowSuccess(string message) => AnsiConsole.MarkupLine($"[green]✔ {message}[/]");
        public static void ShowError(string message) => AnsiConsole.MarkupLine($"[red]❌ {message}[/]");

        // 💾 Spar-animation
        public static void ShowSaveAnimation()
        {
            Console.Clear();
            AnsiConsole.Write(new FigletText("Saving Data...").Centered().Color(Color.Yellow));

            AnsiConsole.Status()
                .Spinner(Spinner.Known.BouncingBar)
                .SpinnerStyle(Style.Parse("green"))
                .Start("Saving league data...", ctx =>
                {
                    Thread.Sleep(800);
                    ctx.Status("Writing files...");
                    Thread.Sleep(800);
                    ctx.Status("Finalizing...");
                    Thread.Sleep(700);
                });

            AnsiConsole.MarkupLine("\n[bold green]✔ All data saved successfully![/]");
            Thread.Sleep(400);

            AnsiConsole.Write(
                new Panel(new Markup($"[bold yellow]Goodbye, Manager![/]\n[grey]Session ended at {DateTime.Now:T} ⚽[/]"))
                    .Header("[bold red]Session Closed[/]")
                    .Border(BoxBorder.Rounded)
                    .BorderColor(Color.Green)
                    .HeaderAlignment(Justify.Center)
                    .Expand());
        }

        // 🤖 AI-generation animation
        public static void ShowAIGenerationAnimation()
        {
            Console.Clear();
            AnsiConsole.Write(
                new FigletText("AI Generating Team")
                    .Centered()
                    .Color(Color.Blue));

            AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots2)
                .SpinnerStyle(Style.Parse("blue"))
                .Start("Booting AI engines...", ctx =>
                {
                    Thread.Sleep(900);
                    ctx.Status("Analyzing Premier League statistics...");
                    Thread.Sleep(1000);
                    ctx.Status("Creating player attributes...");
                    Thread.Sleep(1000);
                    ctx.Status("Balancing team chemistry...");
                    Thread.Sleep(1000);
                    ctx.Status("Finalizing lineup...");
                    Thread.Sleep(800);
                });

            AnsiConsole.Progress()
                .Columns(new ProgressColumn[]
                {
                    new TaskDescriptionColumn(),
                    new ProgressBarColumn(),
                    new PercentageColumn()
                })
                .Start(ctx =>
                {
                    var task = ctx.AddTask("[yellow]Training Neural Network[/]");
                    while (!task.IsFinished)
                    {
                        task.Increment(5);
                        Thread.Sleep(60);
                    }
                });

            Thread.Sleep(400);
            AnsiConsole.MarkupLine("\n[bold cyan]🤖 AI systems ready — generating team data...[/]");
        }

        // 🧹 Reset-bekräftelse
        public static bool ConfirmReset()
        {
            var confirm = AnsiConsole.Confirm("[bold red]⚠ Are you sure you want to reset all league data?[/]");
            if (confirm)
            {
                AnsiConsole.MarkupLine("[yellow]🔄 Reset confirmed![/]");
                Thread.Sleep(400);
                return true;
            }

            AnsiConsole.MarkupLine("[grey]Reset cancelled.[/]");
            Thread.Sleep(400);
            return false;
        }

        // 🧹 Reset-animation
        public static void ShowResetAnimation()
        {
            Console.Clear();
            AnsiConsole.Write(new FigletText("Resetting League").Centered().Color(Color.Red));

            AnsiConsole.Status()
                .Spinner(Spinner.Known.Moon)
                .SpinnerStyle(Style.Parse("red"))
                .Start("Deleting old data...", ctx =>
                {
                    Thread.Sleep(800);
                    ctx.Status("Clearing players...");
                    Thread.Sleep(900);
                    ctx.Status("Reinitializing league files...");
                    Thread.Sleep(800);
                });

            AnsiConsole.MarkupLine("\n[green]✔ League data reset successfully![/]");
        }
    }
}
