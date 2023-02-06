using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Queries;
using BuberDinner.Application.Services.Authentication;
using Microsoft.AspNetCore.Mvc;
using BuberDinner.Contracts.Authentication;
using BuberDinner.Domain.Common.Errors;
using MediatR;
using ErrorOr;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;

namespace BuberDinner.Api.Controllers;

[Route("auth")]
[AllowAnonymous]
// [ErrorHandlingFilter] //Could add error handling here
public class AuthenticationController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public AuthenticationController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        // OneOf<AuthenticationResult, IError> registerResult = _authenticationService.Register(
        //     request.FirstName,
        //     request.LastName,
        //     request.Email,
        //     request.Password
        // );

        // return registerResult.Match(
        //     authResult => Ok(MapAuthResult(authResult)),
        //     error => Problem(statusCode: (int)error.StatusCode, title: error.ErrorMessage)
        // );

        // Result<AuthenticationResult> registerResult = _authenticationService.Register(
        //     request.FirstName,
        //     request.LastName,
        //     request.Email,
        //     request.Password
        // );

        // if (registerResult.IsSuccess)
        // {
        //     return Ok(MapAuthResult(registerResult.Value));
        // }

        // var firstError = registerResult.Errors[0];
        // if (firstError is DuplicateEmailError)
        // {
        //     return Problem(statusCode: StatusCodes.Status409Conflict, title: "Im sure it'");
        // }
        //
        // return Problem();

        // var command = new RegisterCommand(
        //     request.FirstName,
        //     request.LastName,
        //     request.Email,
        //     request.Password
        // );

        var command = _mapper.Map<TRequest>(request);    
        
        var authResult = await _mediator.Send(command);

        return authResult.Match(
            result => Ok(_mapper.Map<AuthenticationResponse>(result)),
            Problem
        );
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = _mapper.Map<LoginQuery>(request);
        var authResult = await _mediator.Send(query);

        if (authResult.IsError && authResult.FirstError == Errors.Authentication.InvalidCredentials)
        {
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: authResult.FirstError.Description);
        }

        return authResult.Match(
            result => Ok(_mapper.Map<AuthenticationResponse>(result)),
            Problem
        );
    }
}