using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.Services.Authentication;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using MediatR;
using ErrorOr;

namespace BuberDinner.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<TRequest, ErrorOr<AuthenticationResult>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public RegisterCommandHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }
    
    public async Task<ErrorOr<AuthenticationResult>> Handle(TRequest comamnd, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        
        //1. Validate the user doesn't exist
        if (_userRepository.GetUserByEmail(comamnd.Email) is not null)
        {
            return Errors.User.DuplicateEmail;
        }

        //2. Create user (generate unique ID) & Persist to DB
        var user = new User
        {
            FirstName = comamnd.FirstName, 
            LastName = comamnd.LastName, 
            Email = comamnd.Email,
            Password = comamnd.Password
        };

        _userRepository.Add(user);

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(
            user,
            token
        );
        
    }
}