using System;
using System.Collections.Generic;
using System.Linq;

namespace PremierLeagueManager
{
    public class Team // Representerar ett lag i Premier League
    {
        public string Name { get; set; }           // Lagets namn
        public string Manager { get; set; }        // Lagets manager
        public List<Player> Players { get; set; }  // Lista över lagets spelare
        public int Points { get; set; }            // Lagets poäng
        public int GoalsFor { get; set; }          // Gjorda mål
        public int GoalsAgainst { get; set; }      // Insläppta mål

        // Beräknad egenskap
        public int GoalDifference => GoalsFor - GoalsAgainst;

        // Parameterlös konstruktor för JSON
        public Team()
        {
            Players = new List<Player>();
        }

        // Konstruktor för manuell skapelse
        public Team(string name, string manager)
        {
            Name = name;
            Manager = manager;
            Players = new List<Player>();
            Points = 0;
            GoalsFor = 0;
            GoalsAgainst = 0;
        }

        // Metod för att uppdatera poäng baserat på matchresultat
        public void RecordMatch(int goalsFor, int goalsAgainst)
        {
            GoalsFor += goalsFor;
            GoalsAgainst += goalsAgainst;

            if (goalsFor > goalsAgainst)
                Points += 3;
            else if (goalsFor == goalsAgainst)
                Points += 1;
        }

        // För snygg utskrift (t.ex. i tabell)
        public override string ToString()
        {
            return $"{Name} ({Manager}) - {Points} pts | GD: {GoalDifference}";
        }
    }
}
