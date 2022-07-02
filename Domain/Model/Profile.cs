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
	[Table("Profile")]
	public class Profile : Entity {
		[Required]
		[MaxLength(250)]
		public string FullName { get; set; }

		[Required]
		public User Owner { get; set; }

		[Required]
		[MaxLength(1024)]
		public string About { get; set; }

		[Required]
		[MaxLength(250)]
		public string WebsiteURL { get; set; }


		public Profile () { /*EF*/ }

		public Profile(string fullName, User owner, string about, string websiteURL) {
			FullName = fullName;
			Owner = owner;
			About = about;
			WebsiteURL = websiteURL;
		}

	}
}
