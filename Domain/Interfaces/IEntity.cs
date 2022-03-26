using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces {
	public interface IEntity {
		public int? Id { get; }
		public bool Deleted { get; set; }
	}
}
