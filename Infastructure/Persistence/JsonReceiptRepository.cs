using ReceiptReader.Domain.Repositories;

namespace ReceiptReader.Infastructure.Persistence;

public class JsonReceiptRepository : IReceiptRepository
{
    private readonly string filePath = Path.Combine(FileSystem.AppDataDirectory, "receipts.json");
    public async Task<List<Receipt>> GetAllAsync()
    {
        if (!File.Exists(filePath))
            return new List<Receipt>();

        var jsonString = await File.ReadAllTextAsync(filePath);
        return JsonSerializer.Deserialize<List<Receipt>>(jsonString) ?? new List<Receipt>();// problim
    }
    public async Task<Receipt?> GetAsync(Guid id)
    {
        var receipts = await GetAllAsync();
        var receiptIndex = receipts.FindIndex(x => x.Id == id);
        return receiptIndex == -1 ? null : receipts[receiptIndex];
    }
    public async Task SaveAsync(Receipt receipt)
    {
        var receipts = await GetAllAsync();
        if (receipts.Contains(receipt))
        {
            if(await ReplaceAsync(receipt))
                await Shell.Current.DisplayAlert("Success", "Receipt edited.", "OK");
        }
        else
        {
            receipts.Add(receipt);
            await SaveAllAsync(receipts);
            await Shell.Current.DisplayAlert("Success", "Receipt saved.", "OK");
        }
    }
    public async Task RemoveAsync(Receipt receipt)
    {
        var receipts = await GetAllAsync();
        if (receipts.Remove(receipt))
            await SaveAllAsync(receipts);
        else
            await Shell.Current.DisplayAlert("Error", "Receipt could not be deleted", "OK");
    }
    public async Task SaveAllAsync(List<Receipt> receipts)
    {
        var json = JsonSerializer.Serialize(receipts);
        await File.WriteAllTextAsync(filePath, json);
    }
    public async Task<bool> EditAsync(Receipt receipt, Item oldItem, Item newItem)
    {
        if(!receipt.Items.Contains(oldItem) || receipt.Items.Contains(newItem)) return false;

        var receipts = await GetAllAsync();
        var receiptIndex = receipts.IndexOf(receipt);
        if (receiptIndex == -1) return false;

        var items = receipts[receiptIndex].Items;
        var itemIndex = items.IndexOf(oldItem);
        if (itemIndex == -1) return false;

        items[itemIndex] = newItem;
        receipts[receiptIndex].Items = items;
        await SaveAllAsync(receipts);
        return true;
    }
    private async Task<bool> ReplaceAsync(Receipt receipt)
    {
        var receipts = await GetAllAsync();
        if(receipt == null) return false;
        if(receipt.Items.Count == 0) return false;
        if(receipts.Contains(receipt))
        {
            var index = receipts.IndexOf(receipt);
            receipts[index] = receipt;
            await SaveAllAsync(receipts);
            return true;
        }
        return false;
    }
}
