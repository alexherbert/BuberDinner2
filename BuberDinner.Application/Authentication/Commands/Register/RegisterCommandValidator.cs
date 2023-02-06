using System.Text.RegularExpressions;
using FluentValidation;

namespace BuberDinner.Application.Authentication.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<TRequest>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}