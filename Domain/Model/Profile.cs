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

		// todo opvullen
		[Required]
		[MaxLength(250)]
		public string FullName { get; set; }

		[Required]
		public bool IsPrimary { get; set; }


		public Profile () { /*EF*/ }

		public Profile(string fullName, bool isPrimary) {
			FullName = fullName;
			IsPrimary = isPrimary;
		}

	}
}
