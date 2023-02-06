using BuberDinner.Domain.Common.Models;
using BuberDinner.Domain.Menu.Entities;
using BuberDinner.Domain.Menu.ValueObjects;

namespace BuberDinner.Domain.Menu;

public sealed class Menu : AggregateRoot<MenuId>
{
    private readonly List<MenuSection> _sections = new();
    public string Name { get; }
    public string Description { get; }
    public float AverageRating { get; }

    public IReadOnlyList<MenuSection> Sections => _sections.AsReadOnly();

    public Menu(MenuId menuId, string name, string description, float averageRating) : base(menuId)
    {
        Name = name;
        Description = description;
        AverageRating = averageRating;
    }

    public static Menu Create(string name, string description, float averageRating)
    {
        return new Menu(MenuId.CreateUnique(), name, description, averageRating);
    }
}