using System;

namespace PremierLeagueManager
{
    public class Player // Representerar en spelare i Premier League
    {
        public string Name { get; set; }      // Spelarens namn
        public string Position { get; set; }  // Position på planen
        public int GoalsScored { get; set; }  // Gjorda mål
        public int Assists { get; set; }      // Assist
        public string Team { get; set; }      // Lagets namn
        public int Rating { get; set; }       // Prestandabetyg

        // Parameterlös konstruktor (för JSON)
        public Player() { }

        // Konstruktor för manuell skapelse
        public Player(string name, string position, int goalsScored, int assists, string team, int rating)
        {
            Name = name;
            Position = position;
            GoalsScored = goalsScored;
            Assists = assists;
            Team = team;
            Rating = rating;
        }

        // Uppdaterar statistik (t.ex. efter en match)
        public void AddStats(int goals, int assists)
        {
            GoalsScored += goals;
            Assists += assists;
        }

        // För snygg utskrift
        public override string ToString()
        {
            return $"{Name} ({Position}) - {GoalsScored}G / {Assists}A | {Team} | ⭐ {Rating}";
        }
    }
}
