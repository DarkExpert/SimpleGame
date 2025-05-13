using Microsoft.AspNetCore.Identity;

namespace SimpleGame.UI.Models
{
	public class ApplicationUser : IdentityUser
	{
		public int GamesPlayed { get; set; }
		public int GamesWon { get; set; }
	}
}
