using Domain.Model.Messaging;
using FluentValidation;
using MediatR;

namespace Logic.Behaviors {
	public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
														where TRequest : IRequest<TResponse>
														where TResponse : Response, new() {

		// Alle validators van toepassing worden ontvangen
		private readonly IEnumerable<IValidator<TRequest>> _validators;
		public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators) {
			_validators = validators;
		}

		public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next) {
			// pre
			// Indien er toepasbare validators zijn voor deze command/query
			if (_validators.Any()) {
				// Aanmaken van een validatiecontext
				var context = new ValidationContext<TRequest>(request);

				// Alle validaties uitvoeren
				var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

				// Aantal validatie failures
				var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

				// Failures verzamelen en met gepaste format returnen
				if (failures.Count > 0) {
					var result = new TResponse();

					result.Messages = failures.Select(f =>
						new Message {
							Body = f.ErrorMessage,
							MessageType = MessageType.Error
						}
					).ToList();

					result.Messages.Add(new Message { Body = "Validation failed, check your request", MessageType = MessageType.Info });

					return result;
				}
			}

			// validatie ok
			return await next();


			// post
		}
	}
}
