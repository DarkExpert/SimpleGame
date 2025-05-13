namespace SimpleGame.UI.Models
{
	public class Player
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public ICollection<Card> Hand { get; set; }
		public int GamesPlayed { get; set; }
		public int GamesWon { get; set; }
	}
}
