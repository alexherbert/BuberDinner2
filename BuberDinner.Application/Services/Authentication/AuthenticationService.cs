using BuberDinner.Application.Common.Errors;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using ErrorOr;
using Result = FluentResults.Result;

// using OneOf;

namespace BuberDinner.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public ErrorOr<AuthenticationResult> Login(string email, string password)
    {
        //1. Validate the user doesn't exist
        if (_userRepository.GetUserByEmail(email) is not User user)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        if (user.Password != password)
        {
            return new[] { Errors.Authentication.InvalidCredentials };
        }
        
        var token = _jwtTokenGenerator.GenerateToken(user);
        
        return new AuthenticationResult(
            user,  
            token
        );
    }

    // public OneOf<AuthenticationResult, IError> Register(string firstName, string lastName, string email, string password)
    // public Result<AuthenticationResult> Register(string firstName, string lastName, string email, string password)
    public ErrorOr<AuthenticationResult> Register(string firstName, string lastName, string email, string password)
    {
        //1. Validate the user doesn't exist
        if (_userRepository.GetUserByEmail(email) is not null)
        {
            // throw new DuplicateEmailException();
            // throw new Exception("User with the given email already exists");
            // return Result.Fail<AuthenticationResult>(new [] {new DuplicateEmailError()} );
            return Errors.User.DuplicateEmail;
        }

        //2. Create user (generate unique ID) & Persist to DB
        var user = new User
        {
            FirstName = firstName, 
            LastName = lastName, 
            Email = email,
            Password = password
        };

        _userRepository.Add(user);

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(
            user,
            token
        );
    }
}