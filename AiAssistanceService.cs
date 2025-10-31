using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PremierLeagueManager
{
    public class AiAssistanceService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        private readonly Random _rng = new Random();

        public AiAssistanceService()
        {
            _apiKey = Environment.GetEnvironmentVariable("AI_API_KEY")
                      ?? throw new InvalidOperationException("❌ AI_API_KEY not found. Use: setx AI_API_KEY \"sk-...\"");

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.openai.com/v1/")
            };
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        // === 🤖 Genererar lagdata via OpenAI ===
        public async Task<Team> GenerateTeamAsync(string teamName)
        {
            string prompt = $@"
You are a football data generator.

Create a JSON object describing the team '{teamName}' in the English Premier League.

The JSON must have this exact structure:
{{
  ""manager"": ""<manager name>"",
  ""points"": <int>,
  ""goalsFor"": <int>,
  ""goalsAgainst"": <int>,
  ""description"": ""<1-sentence summary>"",
  ""players"": [
    {{ ""name"": ""<player1>"", ""position"": ""<pos>"", ""goals"": <int>, ""assists"": <int>, ""rating"": <int 70-95> }},
    ...
  ]
}}

Rules:
- Always include exactly 5 players.
- Ratings must be realistic for Premier League players (70–95).
- Output ONLY valid JSON, no markdown or ```json fences.
";

            var requestBody = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new { role = "system", content = "You output ONLY valid JSON following the given structure. No markdown, no comments." },
                    new { role = "user", content = prompt }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("chat/completions", content);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseString);
            string contentText = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            // 🧹 Rensa bort ```json fences om de finns
            string cleaned = CleanupJson(contentText);

            try
            {
                using var teamJson = JsonDocument.Parse(cleaned);
                var root = teamJson.RootElement;

                var team = new Team
                {
                    Name = teamName,
                    Manager = root.TryGetProperty("manager", out var mgr) ? mgr.GetString() ?? "Unknown Manager" : "Unknown Manager",
                    Points = root.TryGetProperty("points", out var pts) ? pts.GetInt32() : _rng.Next(20, 60),
                    GoalsFor = root.TryGetProperty("goalsFor", out var gf) ? gf.GetInt32() : _rng.Next(15, 60),
                    GoalsAgainst = root.TryGetProperty("goalsAgainst", out var ga) ? ga.GetInt32() : _rng.Next(10, 50),
                    Players = new List<Player>()
                };

                // 🔹 Kort lagbeskrivning
                if (root.TryGetProperty("description", out var desc))
                {
                    string text = desc.GetString() ?? "No description available.";
                    AnsiConsole.MarkupLine($"[cyan]\n🏟️  {Markup.Escape(text)}[/]\n");
                }

                // 🔹 Spelare
                if (root.TryGetProperty("players", out var playersEl) && playersEl.ValueKind == JsonValueKind.Array)
                {
                    foreach (var p in playersEl.EnumerateArray())
                    {
                        var player = new Player
                        {
                            Name = p.TryGetProperty("name", out var pn) ? pn.GetString() ?? "Unknown Player" : "Unknown Player",
                            Position = p.TryGetProperty("position", out var pos) ? pos.GetString() ?? "Unknown" : "Unknown",
                            GoalsScored = p.TryGetProperty("goals", out var g) ? g.GetInt32() : 0,
                            Assists = p.TryGetProperty("assists", out var a) ? a.GetInt32() : 0,
                            Team = teamName,
                            Rating = p.TryGetProperty("rating", out var r) ? r.GetInt32() : 70
                        };
                        team.Players.Add(player);
                    }
                }

                // 🔹 Om AI gav tom lista → fallbackspelare
                if (team.Players.Count == 0)
                {
                    team.Players = GenerateFallbackPlayers(teamName);
                }

                // 🧠 Justera ratingar till realistiskt intervall
                NormalizeRatings(team.Players);

                return team;
            }
            catch (Exception ex)
            {
                throw new Exception($"❌ Failed to parse AI JSON response:\n{cleaned}\n\n{ex.Message}");
            }
        }

        // === Hjälpmetod: ta bort ```json fences ===
        private static string CleanupJson(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return "{}";

            string trimmed = raw.Trim();

            if (trimmed.StartsWith("```"))
            {
                var lines = trimmed.Split('\n');
                trimmed = string.Join('\n', lines, 1, lines.Length - 1);
                trimmed = trimmed.Trim();
                if (trimmed.EndsWith("```"))
                    trimmed = trimmed.Substring(0, trimmed.Length - 3);
            }

            return trimmed.Trim();
        }

        // === Hjälpmetod: fallbackspelare ===
        private static List<Player> GenerateFallbackPlayers(string teamName)
        {
            return new List<Player>
            {
                new Player("Player A", "Forward", 8, 3, teamName, 82),
                new Player("Player B", "Midfielder", 4, 6, teamName, 79),
                new Player("Player C", "Defender", 1, 1, teamName, 76),
                new Player("Player D", "Winger", 5, 2, teamName, 80),
                new Player("Player E", "Goalkeeper", 0, 0, teamName, 78)
            };
        }

        // === Hjälpmetod: justera ratingar (realistiskt intervall per position) ===
        private void NormalizeRatings(List<Player> players)
        {
            foreach (var p in players)
            {
                string pos = p.Position.ToLower();

                if (p.Rating < 70 || p.Rating > 99)
                {
                    switch (pos)
                    {
                        case "goalkeeper":
                            p.Rating = _rng.Next(75, 90);
                            break;
                        case "defender":
                            p.Rating = _rng.Next(73, 88);
                            break;
                        case "midfielder":
                            p.Rating = _rng.Next(75, 92);
                            break;
                        case "forward":
                        case "winger":
                            p.Rating = _rng.Next(78, 95);
                            break;
                        default:
                            p.Rating = _rng.Next(70, 90);
                            break;
                    }
                }
                else
                {
                    if (pos.Contains("forward") && p.Rating < 80)
                        p.Rating += _rng.Next(3, 10);
                    if (pos.Contains("defender") && p.Rating > 90)
                        p.Rating -= _rng.Next(3, 7);
                }
            }
        }

        // === 🧠 Interaktiv metod med snygg tabell + totals ===
        public async Task GenerateTeamInteractiveAsync(LeagueService leagueService)
        {
            MenuHelper.ShowSectionTitle("🤖 Generate Team with AI");
            string teamName = AnsiConsole.Ask<string>("Enter Premier League team name:");

            try
            {
                var generatedTeam = await GenerateTeamAsync(teamName);
                leagueService.AddTeamFromAI(generatedTeam);

                // 🎨 Visa snygg tabell
                AnsiConsole.MarkupLine($"\n[bold green]Team generated successfully![/]");
                AnsiConsole.MarkupLine($"[yellow]{generatedTeam.Name}[/] - [grey]{generatedTeam.Manager}[/]\n");

                var table = new Table()
                    .AddColumn("[bold]Player[/]")
                    .AddColumn("Position")
                    .AddColumn("Goals")
                    .AddColumn("Assists")
                    .AddColumn("Rating");

                int totalGoals = 0;
                int totalAssists = 0;

                foreach (var p in generatedTeam.Players)
                {
                    totalGoals += p.GoalsScored;
                    totalAssists += p.Assists;

                    string ratingColor = p.Rating switch
                    {
                        >= 90 => "bold lime",
                        >= 80 => "green",
                        >= 70 => "yellow",
                        _ => "red"
                    };

                    table.AddRow(
                        $"[white]{Markup.Escape(p.Name)}[/]",
                        $"{Markup.Escape(p.Position)}",
                        $"{p.GoalsScored}",
                        $"{p.Assists}",
                        $"[{ratingColor}]{p.Rating}[/]"
                    );
                }

                table.Border(TableBorder.Rounded);
                table.Centered();

                AnsiConsole.Write(table);

                // 🔢 Totalsummering
                AnsiConsole.MarkupLine($"\n[bold cyan]Team Totals:[/] [yellow]{totalGoals}[/] goals, [yellow]{totalAssists}[/] assists");
                AnsiConsole.MarkupLine($"[green]✔ {generatedTeam.Players.Count} players generated for {generatedTeam.Name}![/]");
            }
            catch (Exception ex)
            {
                MenuHelper.ShowError(ex.Message);
            }
        }
    }
}
