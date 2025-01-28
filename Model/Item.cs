using System.Text;

namespace ReceiptReader.Model;
public class Item
{
    public string Name { get; set; }
    public double Value { get; set; }
    public string StringValue { get; set; }

    public Item(string newline)
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

        Value = double.Parse(sb.ToString()) / 100; // format culture could be faster
        StringValue = sb.ToString();
    }
}
//quantity?
//total value?

/*
*/
