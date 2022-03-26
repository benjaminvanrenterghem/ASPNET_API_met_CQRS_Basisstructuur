using Domain.Model.Messaging;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

					/* Zorgt voor bv volgende response (handler wordt niet bereikt)
						{
							  "content": null,
							  "success": false,
							  "messages": [
								{
								  "type": 2,
								  "body": "'Office Request DT O Id' must be empty."
								},
								{
								  "type": 0,
								  "body": "Validation failed, check your request"
								}
							  ]
						}
					*/

					var result = new TResponse();

					result.Messages = failures.Select(f =>
						new Message {
							Body = f.ErrorMessage,
							MessageType = MessageType.Error
						}
					).ToList();

					result.Messages.Add(new Message { Body = "Validation failed, check your request", MessageType = MessageType.Info });

					// direct returnen, zonder next (handlers worden niet bereikt)
					return result;
				}
			}

			// validatie ok
			return await next();


			// post
		}
	}
}
