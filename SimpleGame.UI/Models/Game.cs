namespace SimpleGame.UI.Models
{
	public class Game
	{
		public int Id { get; set; }
		public List<Card> Cards { get; set; } = new List<Card>();
		public string CurrentTurn { get; set; }
		public bool IsGameOver { get; set; }
		public string Winner { get; set; } = string.Empty;
	}
}
