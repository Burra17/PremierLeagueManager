PremierLeagueManager/
├── Models/
│   ├── Team.cs              # Klass för lag (namn, poäng, målskillnad)
│   └── Player.cs            # Klass för spelare (namn, position, mål, assist)
│
├── Services/
│   ├── DataStore.cs         # Generisk klass för JSON-hantering
│   └── LeagueService.cs     # Logik för tabell, statistik och sortering
│
├── UI/
│   └── MenuHelper.cs        # Menyer och UI med Spectre.Console
│
├── Program.cs               # Huvudfil som startar programmet
│
└── data/
    ├── teams.json           # Sparad data för lag
    └── players.json         # Sparad data för spelare
