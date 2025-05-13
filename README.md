# SimpleGame
# Go Fish - Razor Pages Game 🎴

This is a simple interactive web version of the Go Fish card game built with ASP.NET Core Razor Pages and Entity Framework Core.

## Features

- 🎮 One-player Go Fish game against AI
- 🃏 Card drawing and matching rules enforced server-side
- 📊 Tracks user statistics: games played and games won
- 🔐 User authentication via ASP.NET Core Identity
- 💽 Game state persisted in SQL Server LocalDB
- ✨ Card animations and responsive UI for mobile

## Technologies

- ASP.NET Core Razor Pages (.NET 8)
- Entity Framework Core (SQL Server LocalDB)
- ASP.NET Identity (custom `ApplicationUser`)
- Bootstrap 5

## Getting Started

1. Clone the repo and open in Visual Studio 2022+
2. Run database migrations:

3. Press F5 to launch the game locally
4. Register an account and start playing!

## Gameplay

- Click a card to ask the AI for that rank
- Draw if AI has none
- Complete books (4-of-a-kind) to remove from your hand
- First to empty hand wins

## Author

Developed by Behnam Maghoul 

