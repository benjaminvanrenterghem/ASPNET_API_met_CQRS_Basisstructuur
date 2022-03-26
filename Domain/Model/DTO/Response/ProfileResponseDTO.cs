using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.DTO.Response {
	public class ProfileResponseDTO : Entity {
		public string FullName { get; set; }
		public bool IsPrimary { get; set; }
	}
}
