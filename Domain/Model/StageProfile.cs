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
		[Required]
		[MaxLength(250)]
		public string FullName { get; set; }

		[Required]
		public virtual User Owner { get; set; }

		// Volgt de EF naming convention en wordt door EF aanschouwt als zijnde gekoppeld aan de property Owner
		// Convention [Entity_PropertyName]+"Id" = OwnerId
		// Laat toe om louter een id mee te geven om een correcte relatie te bekomen (zie UpdateCommandHandlers)
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
