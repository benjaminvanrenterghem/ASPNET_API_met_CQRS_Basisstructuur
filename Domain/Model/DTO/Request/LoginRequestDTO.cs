using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.DTO.Request {
	public class LoginRequestDTO {
		public string Email { get; set; }
		public string Password { get; set; }
	}
}
