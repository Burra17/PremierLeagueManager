using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PremierLeagueManager
{
    public class Player // Representerar en spelare i Premier League
    {
        public string Name { get; set; } // Spelarens namn
        public string Position { get; set; } // Spelarens position på planen
        public int GoalsScored { get; set; } // Antal mål som spelaren har gjort
        public int Assists { get; set; } // Antal assist som spelaren har gjort
        public string Team { get; set; } // Namnet på spelarens lag
        public int Rating { get; set; } // Spelarens betyg baserat på prestationer

        // Konstruktor för att initiera en spelare med nödvändig information
        public Player(string name, string position, int goalsScored, int assists, string team, int rating)
        {
            Name = name;
            Position = position;
            GoalsScored = goalsScored;
            Assists = assists;
            Team = team;
            Rating = rating;
        }
    }
}
