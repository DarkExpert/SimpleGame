using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using SimpleGame.UI.Models;
using System.Collections.Generic;

namespace SimpleGame.UI.Data
{
	public class GameDbContext : IdentityDbContext<ApplicationUser>
	{
		public GameDbContext(DbContextOptions<GameDbContext> options) : base(options) { }

		public DbSet<Card> Cards { get; set; }
		public DbSet<Game> Games { get; set; }
		public DbSet<Player> Players { get; set; }
	}
}
