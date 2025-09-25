using System.Text;
using System.Text.Json.Serialization;

namespace ReceiptReader.Domain.Entities;
public class Item
{
    public string Name { get; set; }
    public double Price { get; set; }

    public Item(string newline)
    {
        ParseFromOcrLine(newline);
    }
    public Item(Item item)
    {
        Name = item.Name;
        Price = item.Price;
    }
    private void ParseFromOcrLine(string newline)
    {
        StringBuilder sb = new();
        bool isNamed = false;
        bool isPrevWhiteSpace = false;

        foreach (char c in newline)
        {
            sb.Append(c);
            if (!isNamed)
            {
                if (isPrevWhiteSpace && char.IsWhiteSpace(c))
                {
                    Name = sb.ToString().Trim();
                    isNamed = true;
                    continue;
                }
                isPrevWhiteSpace = char.IsWhiteSpace(c);
            }
        }

        for (int i = sb.Length - 1; i > 0; --i)
        {
            if (sb[i] == ' ') // ocr sometimes sees a space where there is none
            {
                if (sb[i - 1] == ',' || sb[i - 1] == '\'') // or an apostrophe instead of a comma
                    continue;

                sb.Remove(0, i); // if the space really is there it means the next char is the first digit of the price
                break;
            }
        }

        sb.Remove(sb.Length - 1, 1);

        sb.Replace(" ", ""); // ocr workaround again

        Price = double.Parse(sb.ToString()) / 100; // format culture could be faster but this works
    }
    [JsonConstructor]
    public Item(string name, double price)
    {
        Name = name;
        Price = price;
    }
    public override bool Equals(object? obj) => obj is Item other && Name == other.Name && Price == other.Price;
}
//quantity?
//total value?

/*
*/
