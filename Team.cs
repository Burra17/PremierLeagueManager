using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PremierLeagueManager
{
    public class Team // Representerar ett lag i Premier League
    {
        public string Name { get; set; } // Lagets namn
        public string Manager { get; set; } // Lagets manager
        public List<Player> Players { get; set; } // Lista över lagets spelare
        public int Points { get; set; } // Lagets poäng i ligan
        public int GoalsFor { get; set; } // Antal mål som laget har gjort
        public int GoalsAgainst { get; set; } // Antal mål som laget har släppt in

        // Konstruktor för att initiera ett lag med nödvändig information
        public Team(string name, string manager)
        {
            Name = name;
            Manager = manager;
            Players = new List<Player>();
            Points = 0;
            GoalsFor = 0;
            GoalsAgainst = 0;
        }
    }
}
