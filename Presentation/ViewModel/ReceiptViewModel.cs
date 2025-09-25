using CommunityToolkit.Mvvm.Messaging;
using ReceiptReader.ApplicationLayer.Messages;
using ReceiptReader.ApplicationLayer.Interfaces;
using ReceiptReader.Domain.Repositories;
using ReceiptReader.Infastructure.Persistence;
using ReceiptReader.Infastructure.Services;

namespace ReceiptReader.Presentation.ViewModel;
[QueryProperty(nameof(Receipt), "Receipt")]
public partial class ReceiptViewModel : BaseViewModel
{
    private readonly OcrService _ocrService;
    private readonly IDialogService _dialogService;

    [ObservableProperty]
    public partial string Name { get; set; }
    [ObservableProperty]
    public partial double Total {  get; set; }
    [ObservableProperty]
    public partial string ImagePath {  get; set; }
    public ObservableCollection<Item> Items { get; set; } = new();

    [ObservableProperty]
    public partial Receipt Receipt { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsReceiptNull))]
    public partial bool IsReceiptNotNull { get; set; }
    public bool IsReceiptNull => !IsReceiptNotNull;

    [ObservableProperty]
    public partial Item? ExpandedItem { get; set; }
    public ReceiptViewModel(OcrService ocrService, IDialogService dialogService) // DI?
    {
        Title = Receipt != null ? Receipt.Name : "New Receipt"; // title?
        _ocrService = ocrService;
        _dialogService = dialogService;
    }
    private void RefreshItems()
    {
        Items.Clear();
        Total = 0;
        foreach (var item in Receipt.Items)
        {
            Items.Add(item);
            Total += item.Price;
        }
    }
    partial void OnReceiptChanged(Receipt value)
    {
        if (value != null)
        {
            IsReceiptNotNull = true;
            RefreshItems();
        }
        else
            IsReceiptNotNull = false;
    }
    [RelayCommand]
    async Task GetImagePath()
    {
        try
        {
            if (IsReceiptNull)
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select Receipt Image",
                    FileTypes = FilePickerFileType.Images
                });
                if (result == null)
                    return;
                ImagePath = result.FullPath;
            }
            else
            {
                if (Receipt.ImagePath == null)
                {
                    await _dialogService.Alert("Error", "Could not find Image path");
                    return;
                }
                ImagePath = Receipt.ImagePath;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await _dialogService.Alert("Error", $"Unable to get image path: {ex.Message}", "OK");
        }

    }

    [RelayCommand]
    async Task GetItemsAsync() // należy dodać odczyt date_time z servera albo z exifa
    {
        if(IsBusy) // imagePath == null;
            return;
        try
        {
            IsBusy = true;

            var items = await _ocrService.GetItems(ImagePath);
            if (items == null) return;

            Receipt = new(items, ImagePath);
            RefreshItems();
            Title = Receipt.Name;
            IsReceiptNotNull = true;
            await CheckTotal();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await _dialogService.Alert("Error", $"Unable to get items: {ex.Message}", "OK");
        }
        finally { IsBusy = false; }
    }
    [RelayCommand]
    async Task AddToCollection()
    {
        try
        {
            WeakReferenceMessenger.Default.Send(new AddReceiptToCollectionMessage(Receipt));
            await Shell.Current.GoToAsync("..");//z tej linijki można zrobić osbną funkcję
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await _dialogService.Alert("Error", $"Unable to add items to collection (itemsVM): {ex.Message}", "OK");
        }
    }
    async Task CheckTotal()
    {
        Total = 0;
        foreach (var item in Receipt.Items)
            Total += item.Price;
        if (Total !=  Receipt.Total)
        {
            Receipt.Total = Total;
            await _dialogService.Alert("OCR Mistake", $"Total is different from the sum of all item values, please adjust them", "OK");
        }
    }
    [RelayCommand]
    async Task ToggleExpand(Item tappedItem)
    {
        try
        {
            if (ExpandedItem == tappedItem)
                ExpandedItem = null;
            else
                ExpandedItem = tappedItem;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await _dialogService.Alert("Error", $"Unable to toggle Item (ItemsVM.ToggleExpand): {ex.Message}", "OK");
        }
    }
    [RelayCommand]
    async Task EditItemName(Item selectedItem)
    {
        try
        {
            var response = await _dialogService.PromptAsync("Edit Item Name", "Please enter a new name for the item", initialValue: selectedItem.Name);
            if (response == null)
            {
                await _dialogService.Alert("Error", "Invalid name");
                return;
            }
            Item updatedItem = new(selectedItem) { Name = response };
            IReceiptRepository receiptRepository = new JsonReceiptRepository();
            if (await receiptRepository.EditAsync(Receipt, selectedItem, updatedItem))
            {
                ExpandedItem = null;
                int i = Receipt.Items.IndexOf(selectedItem);
                Receipt.Items[i] = updatedItem;
                RefreshItems();
            }
            else
                await _dialogService.Alert("Error", "Could not edit the receipt");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await _dialogService.Alert("Error", $"Unable to edit item name (ItemsVM.EditItemName): {ex.Message}", "OK");
        }
    }
    [RelayCommand]
    async Task EditItemPrice(Item selectedItem)
    {
        try
        {
            var response = await _dialogService.PromptAsync("Edit Item price", "Please enter a new price for the item", initialValue: selectedItem.Price.ToString());
            if (!double.TryParse(response, out var price))
                return;
            if (response == null || price <= 0)
            {
                await _dialogService.Alert("Error", "Invalid value");
                return;
            }
            Item updatedItem = new(selectedItem) { Price = price };
            IReceiptRepository receiptRepository = new JsonReceiptRepository();
            if (await receiptRepository.EditAsync(Receipt, selectedItem, updatedItem))
            {
                ExpandedItem = null;
                int i = Receipt.Items.IndexOf(selectedItem);
                Receipt.Items[i] = updatedItem;
                RefreshItems();
                await CheckTotal();
            }
            else
                await _dialogService.Alert("Error", "Could not edit the receipt");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await _dialogService.Alert("Error", $"Unable to edit item price (ItemsVM.EditItemPrice): {ex.Message}", "OK");
        }
    }
}
