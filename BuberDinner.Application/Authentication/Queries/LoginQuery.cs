using BuberDinner.Application.Services.Authentication;
using MediatR;
using ErrorOr;

namespace BuberDinner.Application.Authentication.Queries;

public record LoginQuery(string Email, string Password) 
    : IRequest<ErrorOr<AuthenticationResult>>;