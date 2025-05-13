using SimpleGame.UI.Models;

namespace SimpleGame.UI.Services
{
	public class GameService
	{
		private static readonly string[] RANKS = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

		public List<Card> GenerateDeck()
		{
			var cards = new List<Card>();
			foreach (var rank in RANKS)
			{
				for (int i = 0; i < 4; i++) 
				{
					cards.Add(new Card { Rank = rank, Owner = "Deck" });
				}
			}
			return cards.OrderBy(c => Guid.NewGuid()).ToList(); 
		}

		public void DealInitialCards(List<Card> deck)
		{
			for (int i = 0; i < 7; i++)
			{
				DrawCard(deck, "Player");
				DrawCard(deck, "Computer");
			}
		}

		public Card DrawCard(List<Card> deck, string player)
		{
			var card = deck.FirstOrDefault(c => c.Owner == "Deck");
			if (card != null)
			{
				card.Owner = player;
			}
			return card;
		}

		public List<string> CheckForBooks(List<Card> cards, string player)
		{
			var grouped = cards.Where(c => c.Owner == player)
							   .GroupBy(c => c.Rank)
							   .Where(g => g.Count() == 4)
							   .ToList();

			var completedBooks = new List<string>();

			foreach (var group in grouped)
			{
				completedBooks.Add(group.Key);
				foreach (var card in group)
					card.Owner = "Book"; 
			}

			return completedBooks;
		}
	}
}
