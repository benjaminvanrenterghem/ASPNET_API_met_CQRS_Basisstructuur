using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL {
	public class NetworkDbContext : DbContext, INetworkDbContext {
		public DbSet<Profile> Profiles { get; set; }
		public DbSet<User> Users { get; set; }

		public NetworkDbContext(DbContextOptions options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			// Ten behoeve van soft-delete functionaliteit worden soft-deleted rows globaal niet geretourneerd bij uitvoer van queries
			// Indien Entity.Deleted=null => false, en vandaar niet opgenomen in de queryfilter
			modelBuilder.Entity<Profile>()
						.HasQueryFilter(x => !x.Deleted);

			modelBuilder.Entity<User>()
						.HasQueryFilter(x => !x.Deleted);
		}


	}
}
