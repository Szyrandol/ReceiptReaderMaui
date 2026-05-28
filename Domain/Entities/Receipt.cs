using System.Text.Json.Serialization;

namespace ReceiptReader.Domain.Entities;

public class Receipt
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public double Total { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? ImagePath { get; set; }
    public List<Item> Items { get; set; }
    //public string NIP { get; set; } //Żeliński poleca, niezbyt skomplikowane, ale OCR nie jest doskonały
    public Category? Category { get; set; }
    public string? Comment { get; set; }
    //public string currencyName { get; set; } potem można dodać żeby jakoś 


    public Receipt(List<Item> items, string imagePath)//, DateTime dateTime) // chyba z datetime ale to potem
    {
        Id = Guid.NewGuid();
        Total = items[^1].Price;
        items.RemoveAt(items.Count - 1);
        //NIP = (items[0].Price * 100).ToString();
        items.RemoveAt(0);
        Items = items;
        CreatedAt = DateTime.Now;
        CreatedBy = "Admin";
        Name = $"{CreatedBy} {Total} {CreatedAt}";
        ImagePath = imagePath;
    }
    [JsonConstructor]
    public Receipt(Guid id, string name, double total, string createdBy, DateTime createdAt, List<Item> items, string imagePath, Category? category = null, string? comment = null)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        Name = name ;
        Total = total ;
        CreatedBy = createdBy ;
        CreatedAt = createdAt;
        Items = items ;
        ImagePath = imagePath ;
        Category = category ?? Category.Misc ;
        Comment = comment ;
    }
    public override bool Equals(object? obj) => obj is Receipt other && (Id == other.Id && CreatedAt == other.CreatedAt);
    public override int GetHashCode() => Id.GetHashCode();
}

