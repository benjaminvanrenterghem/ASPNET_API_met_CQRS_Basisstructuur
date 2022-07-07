using FluentValidation;
using Logic.Mediated.Commands.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// todo crit add rules in all validators (all files made)
namespace Logic.Behaviors.Validators.ForAuthentication {
	public class CreateJWTTokenCommandValidator : AbstractValidator<CreateJWTTokenCommand> {
	}
}
