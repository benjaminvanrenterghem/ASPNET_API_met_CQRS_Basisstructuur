using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Messaging {
	// Wordt gebruikt in de catch in controllers
	public class FallbackResponse {
		public bool Success => false;
		public List<Message> Messages { get; set; } = new();

		public FallbackResponse(string message) {
			Messages.Add(new() { Body = message });
		}
	}
}
