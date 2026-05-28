namespace ReceiptReader.ApplicationLayer.Interfaces;

public interface IOcrService
{
    Task<List<Item>> GetItems(string imagePath);
    //List<Item> JsonStringToItemList(string jsonText);
}
