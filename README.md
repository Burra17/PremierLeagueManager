# 🏆 Premier League Manager

> Ett färgstarkt och interaktivt **C#-konsolprogram** där du kan hantera Premier League-lag, spelare och statistik —  
> komplett med färg, animationer, JSON-lagring och AI-genererade lag via **OpenAI API** ⚽🤖

---

## 📖 Projektöversikt

**PremierLeagueManager** är byggt i **.NET 8 / C#** och skapades som ett skol- och portföljprojekt.  
Programmet kombinerar klassisk statistikhantering med modern terminaldesign och AI-generering.  

Du kan:
- 🧱 Skapa och hantera lag & spelare  
- 📊 Visa ligatabeller och topplistor  
- 💾 Spara och läsa data från JSON-filer  
- 🤖 Låta AI skapa lag automatiskt via OpenAI:s GPT-4o-modell  
- 🎨 Uppleva ett snyggt och färggrant terminalgränssnitt med **Spectre.Console**

---

## ✨ Funktioner

| Typ | Beskrivning |
|-----|--------------|
| 💅 **Färgrikt UI** | Byggt med *Spectre.Console* för färg, tabeller och animationer |
| 🧾 **JSON-lagring** | Sparar all data lokalt i `data/teams.json` och `data/players.json` |
| 🤖 **AI-lagbyggare** | Genererar lag, manager och spelare via OpenAI |
| 🧹 **Reset-funktion** | Rensar ligadata med bekräftelse och animation |
| 💾 **Save & Exit** | Sparar och avslutar med snygg animation |
| 📊 **Statistik** | Visar tabeller, målgörare och lagpoäng |

---

## 🧰 Teknikstack

- **C# / .NET 8.0**  
- **Spectre.Console** – för färg, tabeller, text och spinners  
- **System.Text.Json** – för JSON-hantering  
- **OpenAI API (GPT-4o-mini)** – för AI-generering av lag och spelare  

---

## ⚙️ Installation

### 1️⃣ Klona projektet

Kör följande i terminalen:

```bash
git clone https://github.com/Burra17/PremierLeagueManager.git
cd PremierLeagueManager
