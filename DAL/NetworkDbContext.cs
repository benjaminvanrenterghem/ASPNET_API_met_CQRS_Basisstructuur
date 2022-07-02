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
		public DbSet<StageProfile> Profiles { get; set; }
		public DbSet<User> Users { get; set; }

		public NetworkDbContext(DbContextOptions options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			// Rijen die ge-softdelete werden maken nooit deel uit van Read statements
			modelBuilder.Entity<StageProfile>()
						.HasQueryFilter(x => !x.Deleted);

			modelBuilder.Entity<User>()
						.HasQueryFilter(x => !x.Deleted);

			// De [TimeStamp] concurrency token bij de RowVersion property in StageProfile en User voorkomt
			// concurrency conflicts bij Update & Delete statements, het toevoegen van meerdere indexen
			// dient een zelfde doel maar met betrekking tot Insert statements
			// ----> Entity.Id hoort hier dus niet bij

			// todo: om erachter te komen of de index het collectief van de 3 is, of deze apart als unique aanzien worden, de check/read in de create stageprofile command disablen en 2 maal aanroepen waarbij slechts 1 prop van waarde verandert, bv sp.WebsiteURL
			// zou een composite index kunnen voorstellen - https://docs.microsoft.com/en-us/ef/core/modeling/indexes?tabs=data-annotations#composite-index
			// indien hij ze samen neemt, deze definitie eens uitproberen 1 prop per keer (3x zelfde dus)

			// slightly relevant https://docs.microsoft.com/en-us/ef/core/modeling/indexes?tabs=data-annotations#included-columns
			modelBuilder.Entity<StageProfile>()
						.HasIndex(sp => new { sp.About, sp.WebsiteURL, sp.FullName })
						
						.IsClustered(false)
						.IsUnique();

			modelBuilder.Entity<User>()
						.HasIndex(user => new { user.Email, user.DisplayName, user.LoginName })
						.IsClustered(false)
						.IsUnique();
		}


	}
}
