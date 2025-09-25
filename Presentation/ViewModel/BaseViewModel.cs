namespace ReceiptReader.Presentation.ViewModel;

//[INotifyPropertyChanged] // 
public partial class BaseViewModel : ObservableObject
{
    public BaseViewModel()
    {

    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    public partial bool IsBusy { get; set; }

    [ObservableProperty]
    public partial string Title { get; set; }

    public bool IsNotBusy => !IsBusy;
}
