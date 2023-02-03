using BuberDinner.Application.Services.Authentication;
using Microsoft.AspNetCore.Mvc;
using BuberDinner.Contracts.Authentication;
using BuberDinner.Domain.Common.Errors;

// using OneOf;

namespace BuberDinner.Api.Controllers;

[Route("auth")]
// [ErrorHandlingFilter] //Could add error handling here
public class AuthenticationController : ApiController
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
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

        var authResult = _authenticationService.Register(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password
        );

        return authResult.Match(
            result => Ok(MapAuthResult(result)),
            Problem
        );
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var authResult = _authenticationService.Login(
            request.Email,
            request.Password
        );

        if (authResult.IsError && authResult.FirstError == Errors.Authentication.InvalidCredentials)
        {
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: authResult.FirstError.Description);
        }

        return authResult.Match(
            result => Ok(MapAuthResult(result)),
            Problem
        );
    }
    
    private static AuthenticationResponse MapAuthResult(AuthenticationResult authResult)
    {
        var authResponse = new AuthenticationResponse(
            authResult.User.Id,
            authResult.User.FirstName,
            authResult.User.LastName,
            authResult.User.Email,
            authResult.Token);

        return authResponse;
    }
}