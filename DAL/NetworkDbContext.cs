using Domain.Interfaces;
using Domain.Model;
using Micro2Go.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
			// Rijen die ge-softdelete werden maken nooit deel uit van Read statements, wel terug te vinden middels bv SSMS
			modelBuilder.Entity<StageProfile>()
						.HasQueryFilter(x => !x.Deleted);

			modelBuilder.Entity<User>()
						.HasQueryFilter(x => !x.Deleted);

			// EF kan geen List<ClearanceLevel> out-of-the-box opslaan, er wordt geconverteerd van en naar
			// de string representatie (List<CL>  <-->  string)
			// Property names kunnen geen komma bevatten, wat dit een veilige operatie maakt
			modelBuilder.Entity<User>()
						.Property<List<ClearanceLevel>>(x => x.ClearanceLevels)
						.HasConversion<string>(
							cls => String.Join(",", cls.ConvertAll(cl => cl.ToString())),
							clsstr => clsstr.Split(",", StringSplitOptions.RemoveEmptyEntries)
											.ToList()
											.ConvertAll(clsstr => Enum.Parse<ClearanceLevel>(clsstr)) 
						)
						// Elke CL max maximum 128 karakters lang zijn, we voorzien plaats voor minimum 250 CLs
						.HasMaxLength(128*250);

			// Onderstaande hoort bij de ConcurrencyToken aanpak in Entity, maar dan voor insert statements
			// Voorkomt insertion van identieke entries door de zeldzame Mediatr.Send racecondition
			// Er wordt bewust geen Composite Index samengesteld aangezien elke property uniek dient te zijn
			// https://docs.microsoft.com/en-us/ef/core/modeling/indexes?tabs=fluent-api
			modelBuilder.Entity<StageProfile>()
						.HasIndex(sp => new { sp.About })
						.IsClustered(false)
						.IsUnique();

			modelBuilder.Entity<StageProfile>()
						.HasIndex(sp => new { sp.WebsiteURL })
						.IsClustered(false)
						.IsUnique();

			modelBuilder.Entity<StageProfile>()
						.HasIndex(sp => new { sp.FullName })
						.IsClustered(false)
						.IsUnique();

			modelBuilder.Entity<User>()
						.HasIndex(user => new { user.Email })
						.IsClustered(false)
						.IsUnique();

			modelBuilder.Entity<User>()
						.HasIndex(user => new { user.DisplayName })
						.IsClustered(false)
						.IsUnique();

			modelBuilder.Entity<User>()
						.HasIndex(user => new { user.LoginName })
						.IsClustered(false)
						.IsUnique();
		}


	}
}
