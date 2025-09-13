using System.Text.Json.Serialization;

namespace ReceiptReader.Model;

public class Receipt
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public double Total { get; set; }
    public string Owner { get; set; }
    public DateTime DateTime { get; set; }
    public string ImagePath { get; set; }
    public List<Item> Items { get; set; }
    //public string currencyName { get; set; } potem można dodać żeby jakoś 
    //public string NIP { get; set; } Żeliński poleca, niezbyt skomplikowane, ale OCR nie jest doskonały


    public Receipt(List<Item> items, string imagePath)//, DateTime dateTime) // chyba z datetime ale to potem
    {
        Id = Guid.NewGuid();
        Total = items[^1].Price;
        items.RemoveAt(items.Count - 1);
        Items = items;
        DateTime = DateTime.Now;
        Owner = "Admin";
        Name = $"{Owner} {Total} {DateTime}";
        ImagePath = imagePath;
    }
    [JsonConstructor]
    public Receipt(Guid id, string name, double total, string owner, DateTime datetime, List<Item> items, string imagePath)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        Name = name ;
        Total = total ;
        Owner = owner ;
        DateTime = datetime ;
        Items = items ;
        ImagePath = imagePath ;
    }
    public override bool Equals(object? obj) => obj is Receipt other && (Id == other.Id || DateTime == other.DateTime);
    public override int GetHashCode() => Id.GetHashCode();
}

