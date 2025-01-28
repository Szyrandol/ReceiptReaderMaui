using Microsoft.Maui.Storage;

namespace ReceiptReader.ViewModel;

public partial class ItemsViewModel : BaseViewModel
{
    OcrService ocrService;
    [ObservableProperty]
    public partial string ImagePath {  get; set; }
    public ObservableCollection<Item> Items { get; } = new();

    public ItemsViewModel(OcrService ocrService)
    {
        Title = "Receipt Reader"; // title?
        this.ocrService = ocrService;
        
    }
    [RelayCommand]
    async Task GetImagePath()
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

    [RelayCommand]
    async Task GetItemsAsync()
    {
        if(IsBusy) // imagePath == null;
            return;
        
        try
        {
            IsBusy = true;
            var items = await ocrService.GetItems(ImagePath);

            if(Items.Count != 0)
                Items.Clear();

            foreach(var item in items)
                Items.Add(item);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error", $"Unable to get items: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
