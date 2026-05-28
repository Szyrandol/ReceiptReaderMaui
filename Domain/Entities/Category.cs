using System.Text.Json.Serialization;
//using System.Windows.Media;

namespace ReceiptReader.Domain.Entities;

public class Category
{
    public string Name { get; init; }
    public string ColorArgbHex { get; init; }

    public Category (Category category)
    {
        Name = category.Name;
        ColorArgbHex = category.ColorArgbHex;
    }
    [JsonConstructor]
    public Category(string name, string colorArgbHex)
    {
        Name = name;
        ColorArgbHex = colorArgbHex;
    }
    public static readonly Category Misc = new("Misc", "#FF8A2BE2");
    public static readonly Category Food = new("Food", "#FFFFA07A");
    public static readonly Category Transport = new("Transport", "#FF6495ED");
    public static readonly Category Health = new("Health", "#FF7CFC00");
    public static readonly Category Entertainment = new("Entertainment", "#FFFF4500");
    public static readonly Category Electronics = new("Electronics", "#FFFFD700");

    public static readonly IReadOnlyList<Category> All =
        [Misc, Food, Transport, Health, Entertainment, Electronics];
    public override bool Equals(object? obj) => obj is Category other && Name == other.Name;
    public override int GetHashCode() => Name.GetHashCode();
}
