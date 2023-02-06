using System.Net;

namespace BuberDinner.Application.Common.Errors;

public class DuplicateEmailException : Exception, IError
{
    public HttpStatusCode StatusCode => HttpStatusCode.Conflict;
    public string ErrorMessage => "Email already exists.";
}