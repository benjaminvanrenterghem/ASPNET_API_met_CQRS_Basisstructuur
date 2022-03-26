using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions {
	public class ExtensionException : Exception {
		public ExtensionException(string? message) : base(message) {
		}

		public ExtensionException(string? message, Exception? innerException) : base(message, innerException) {
		}
	}
}
