using Domain.Abstract;
using Domain.Static;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model {
	[Table("StageProfile")]
	public class StageProfile : Entity {
		// ConcurrencyToken
		// https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/concurrency?view=aspnetcore-6.0#add-a-tracking-property
		[Timestamp]
		public byte[] RowVersion { get; set; }

		[Required]
		[MaxLength(250)]
		public string FullName { get; set; }

		[Required]
		public User Owner { get; set; }

		// Hoort automatisch bij Owner door EF, laat toe om de relatie in te stellen zonder Owner/User te providen
		public int OwnerId { get; set; }

		[Required]
		[MaxLength(1024)]
		public string About { get; set; }

		[Required]
		[MaxLength(250)]
		public string WebsiteURL { get; set; }


		public StageProfile () { /*EF*/ }

		public StageProfile(string fullName, User owner, string about, string websiteURL) {
			FullName = fullName;
			Owner = owner;
			About = about;
			WebsiteURL = websiteURL;
		}

		public StageProfile(string fullName, int ownerId, string about, string websiteURL) {
			FullName = fullName;
			OwnerId = ownerId;
			About = about;
			WebsiteURL = websiteURL;
		}

	}
}
