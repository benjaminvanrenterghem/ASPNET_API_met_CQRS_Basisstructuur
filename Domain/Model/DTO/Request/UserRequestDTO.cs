using Micro2Go.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.DTO.Request {
	public class UserRequestDTO {
		public int? Id { get; set; }
		public string DisplayName { get; set; }
		public string LoginName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public List<string> ClearanceLevels { get; set; } = new() { { ClearanceLevel.User.ToString() } };
	}
}
