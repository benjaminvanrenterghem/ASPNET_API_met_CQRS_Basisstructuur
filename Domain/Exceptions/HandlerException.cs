
// Hier dient karig mee om gesprongen te worden, de hoofdzakelijke wijze van foutafhandeling in handlers is immers om een Response te returnen en hier errors aan te voegen middels .AddError(string)
// Wordt louter gebruikt in CreateJWTTokenCommand
namespace Domain.Exceptions {
	public class HandlerException : Exception {
		public HandlerException(string? message) : base(message) {
		}

		public HandlerException(string? message, Exception? innerException) : base(message, innerException) {
		}
	}
}
