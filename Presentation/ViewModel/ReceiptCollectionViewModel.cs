using CommunityToolkit.Mvvm.Messaging;
using ReceiptReader.ApplicationLayer.Messages;
using ReceiptReader.ApplicationLayer.Services;
using ReceiptReader.Domain.Repositories;

namespace ReceiptReader.Presentation.ViewModel;

public partial class ReceiptCollectionViewModel : BaseViewModel
{
    ReceiptService receiptService;
    IReceiptRepository _receiptRepository;
    public ObservableCollection<Receipt> Receipts { get; set; }
    [ObservableProperty]
    public partial string Owner { get; set; }
    [ObservableProperty]
    public partial Receipt ExpandedReceipt { get; set; }
    //some user verification in the end should be here
    public ReceiptCollectionViewModel(ReceiptService receiptService, IReceiptRepository receiptRepository)
    {
        //if (internet connection) pobierz dane z serwera else wez dane z nosql pg db urządzenia 
        Title = "Receipt Collection";
        this.receiptService = receiptService;
        _receiptRepository = receiptRepository;

        this.Owner = "Admin";
        Receipts = new();
        _ = GetAllFromRepository();

        WeakReferenceMessenger.Default.Register<AddReceiptToCollectionMessage>(this, (r, m) =>
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Add(m.Value);
            });
        });
    }
    [RelayCommand]
    async Task Create()
    {
        await Shell.Current.GoToAsync(nameof(ReceiptPage));
    }
    [RelayCommand]
    async Task GetReceiptsFromUserName() // every owner occurence has to be changed to user, when its finished
    {
        if(IsBusy) return;

        try
        {
            IsBusy = true;
            var receipts = await receiptService.GetReceiptsAsync(Owner);
            if (receipts == null) return;
            Receipts = new ObservableCollection<Receipt>(receipts);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error", $"Unable to get items: {ex.Message}", "OK");
        }
        finally { IsBusy = false; }

    }
    [RelayCommand]
    async Task GetAllFromRepository()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            var receipts = await _receiptRepository.GetAllAsync();
            Receipts.Clear();
            foreach(var r  in receipts)
                Receipts.Add(r);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error", $"Unable to add receipt (ReceiptsVM.GetAllFromRepository): {ex.Message}", "OK");
        }
        finally { IsBusy = false; }
    }
    [RelayCommand]
    async Task Add(Receipt r)
    {
        try
        {
            await _receiptRepository.SaveAsync(r);
            await GetAllFromRepository();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error", $"Unable to add receipt (ReceiptsVM.Add): {ex.Message}", "OK");
        }
    }
    [RelayCommand]
    async Task Delete(Receipt receipt)
    {
        try
        {
            if (!Receipts.Contains(receipt))
                return;
            else
            {
                await _receiptRepository.RemoveAsync(receipt); // this line should be inside of an if
                Receipts.Remove(receipt);
                await _receiptRepository.GetAllAsync();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error", $"Unable to delete receipt (ReceiptsVM.Delete): {ex.Message}", "OK");
        }
    }
    [RelayCommand]
    async Task Edit(Receipt selectedReceipt)
    {
        try
        {
            await Shell.Current.GoToAsync(nameof(ReceiptPage), true,
                new Dictionary<string, object>
                {
                    { "Receipt", selectedReceipt }
                });
            ExpandedReceipt = null;
            await GetAllFromRepository();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error", $"Unable to edit receipt (ReceiptsVM.Edit): {ex.Message}", "OK");
        }
    }
    [RelayCommand]
    async Task ToggleExpand(Receipt tappedReceipt)
    {
        try
        {
            if (ExpandedReceipt == tappedReceipt)
                ExpandedReceipt = null;
            else
                ExpandedReceipt = tappedReceipt;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error", $"Unable to toggle receipt (ReceiptsVM.ToggleExpand): {ex.Message}", "OK");
        }
    }
}
