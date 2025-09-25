namespace ReceiptReader.Domain.Repositories;

public interface IReceiptRepository
{
    Task<List<Receipt>> GetAllAsync();
    Task<Receipt?> GetAsync(Guid id);
    Task SaveAsync(Receipt receipt);
    Task RemoveAsync(Receipt receipt);
    Task SaveAllAsync(List<Receipt> receipts);
    Task<bool> EditAsync(Receipt receipt, Item previous, Item current);
    //Task<bool> ReplaceAsync(Receipt old, Receipt current);
}
