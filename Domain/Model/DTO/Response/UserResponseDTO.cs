using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.DTO.Response {
	public class UserResponseDTO {
		public string DisplayName { get; set; }
		public string LoginName { get; set; }
		public string Email { get; set; }
		// todo check automapping implication of hardcoded property
		public string Password => "Redacted";
		public DateTime RegistrationDate { get; set; }
		public List<string> ClearanceLevels { get; set; }
	}
}
