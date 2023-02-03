namespace BuberDinner.Application.Common.Interfaces.Interfaces;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}