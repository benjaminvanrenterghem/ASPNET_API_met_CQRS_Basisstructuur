using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.DTO.Request {
	public class ProfileRequestDTO {
		public int? Id { get; set; }
		public string FullName { get; set; }
		public int OwnerUserId { get; set; }
		public string About { get; set; }
		public string WebsiteURL { get; set; }
		public bool? Deleted { get; set; }
	}
}
