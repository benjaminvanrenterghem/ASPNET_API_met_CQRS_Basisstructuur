using Domain.Abstract;
using Domain.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.DTO.Response {
	public class ProfileResponseDTO {
		public int Id { get; set; }

		[LocalizedDisplayName("Naam")]
		public string FullName { get; set; }
		public bool IsPrimary { get; set; }
		public bool Deleted { get; set; }
	}
}
