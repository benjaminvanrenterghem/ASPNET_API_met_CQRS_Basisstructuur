using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Messaging {
	public enum MessageType {
		Info,
		Warning,
		Error,
		Deprecated
	}
	
	public class Message {
		public MessageType MessageType { get; set; } = MessageType.Error;
		public string Type => MessageType.ToString();
		public string Body { get; set; }
	}


}
