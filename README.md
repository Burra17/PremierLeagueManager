# 🏆 Premier League Manager

> Ett färgstarkt och interaktivt **C#-konsolprogram** där du kan hantera Premier League-lag, spelare och statistik —  
> komplett med färg, animationer, JSON-lagring och AI-genererade lag via **OpenAI API** ⚽🤖

---

## 🚀 Snabbstart

Vill du bara testa programmet direkt? Följ dessa fyra enkla steg 👇

```bash
# 1️⃣ Klona projektet
git clone https://github.com/Burra17/PremierLeagueManager.git
cd PremierLeagueManager

# 2️⃣ Installera Spectre.Console
dotnet add package Spectre.Console

# 3️⃣ Lägg till din OpenAI API-nyckel (obligatoriskt för AI-funktioner)
[System.Environment]::SetEnvironmentVariable("AI_API_KEY", "sk-din-egen-nyckel-här", "User")

# 4️⃣ Kör programmet
dotnet run
