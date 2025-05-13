using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleGame.UI.Data;
using SimpleGame.UI.Models;
using SimpleGame.UI.Services;

namespace SimpleGame.UI.Pages
{
	public class IndexModel : PageModel
	{
		private readonly GameDbContext _db;
		private readonly GameService _service;
		private readonly ILogger<IndexModel> _logger;
		private readonly UserManager<ApplicationUser> _userManager;

		public Game Game { get; set; }
		public List<Card> PlayerHand { get; set; } = new();
		public string Message { get; set; }
		public int GamesPlayed { get; set; }
		public int GamesWon { get; set; }

		public IndexModel(ILogger<IndexModel> logger, GameDbContext db, GameService service, UserManager<ApplicationUser> userManager)
		{
			_logger = logger;
			_db = db;
			_service = service;
			_userManager = userManager;
		}



		public async Task<IActionResult> OnGetAsync()
		{
			LoadGame();

			var user = await _userManager.GetUserAsync(User);
			if (user != null)
			{
				GamesPlayed = user.GamesPlayed;
				GamesWon = user.GamesWon;
			}

			return Page();
		}
		public async Task<IActionResult> OnPostAsync(string requestedRank)
		{
			LoadGame();

			if (Game == null || Game.IsGameOver) return RedirectToPage();

			
			var playerCards = _db.Cards.Where(c => c.Owner == "Player").ToList();
			var computerCards = _db.Cards.Where(c => c.Owner == "Computer").ToList();

			if (string.IsNullOrEmpty(requestedRank) || !playerCards.Any(c => c.Rank == requestedRank))
			{
				Message = "Invalid request.";
				return Page();
			}

			var matches = computerCards.Where(c => c.Rank == requestedRank).ToList();
			if (matches.Any())
			{
				foreach (var m in matches)
					m.Owner = "Player";
				Message = $"You got {matches.Count} card(s) of {requestedRank}!";
			}
			else
			{
				var drawn = _service.DrawCard(_db.Cards.ToList(), "Player");
				if (drawn != null)
					Message = $"Computer had none. You drew a {drawn.Rank}.";
			}

			var newBooks = _service.CheckForBooks(_db.Cards.ToList(), "Player");
			if (newBooks.Any())
				Message += $" | You completed book(s): {string.Join(", ", newBooks)}";

			
			var playerLeft = _db.Cards.Count(c => c.Owner == "Player");
			if (playerLeft == 0)
			{
				Game.IsGameOver = true;
				Game.Winner = "Player";
				await _db.SaveChangesAsync();
				return RedirectToPage();
			}

			Game.CurrentTurn = "Computer";
			_db.SaveChanges();

			await DoComputerMove();

			return RedirectToPage();
		}

		public async Task<IActionResult> OnPostNewGameAsync()
		{
			
			_db.Cards.RemoveRange(_db.Cards);
			_db.Games.RemoveRange(_db.Games);
			await _db.SaveChangesAsync();

			var game = new Game { CurrentTurn = "Player" };
			var deck = _service.GenerateDeck();

			_service.DealInitialCards(deck);

			foreach (var card in deck) _db.Cards.Add(card);
			_db.Games.Add(game);
			await _db.SaveChangesAsync();

			return RedirectToPage();
		}

		private void LoadGame()
		{
			Game = _db.Games.FirstOrDefault();
			PlayerHand = _db.Cards.Where(c => c.Owner == "Player").ToList();
		}

		private async Task DoComputerMove()
		{
			await Task.Delay(500);

			var allCards = _db.Cards.ToList();
			var computerHand = allCards.Where(c => c.Owner == "Computer").ToList();
			var playerHand = allCards.Where(c => c.Owner == "Player").ToList();

			if (!computerHand.Any())
				return;

			var random = new Random();
			var randomRank = computerHand[random.Next(computerHand.Count)].Rank;

			var matches = playerHand.Where(c => c.Rank == randomRank).ToList();

			if (matches.Any())
			{
				foreach (var m in matches)
					m.Owner = "Computer";

				Message += $" | AI got {matches.Count} of your {randomRank}.";
			}
			else
			{
				var drawn = _service.DrawCard(allCards, "Computer");
				if (drawn != null)
					Message += $" | AI drew a {drawn.Rank}.";
			}

			var newBooks = _service.CheckForBooks(allCards, "Computer");
			if (newBooks.Any())
				Message += $" | AI completed book(s): {string.Join(", ", newBooks)}";

			var playerLeft = allCards.Count(c => c.Owner == "Player");
			var computerLeft = allCards.Count(c => c.Owner == "Computer");

			if (playerLeft == 0 || computerLeft == 0)
			{
				Game.IsGameOver = true;
				Game.Winner = playerLeft == 0 ? "Player" : "Computer";

				
				var user = await _userManager.GetUserAsync(User);
				if (user != null)
				{
					user.GamesPlayed += 1;
					if (Game.Winner == "Player")
						user.GamesWon += 1;

					await _userManager.UpdateAsync(user);
				}
			}
			else
			{
				Game.CurrentTurn = "Player";
			}

			await _db.SaveChangesAsync();
		}

	}
}

