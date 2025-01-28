namespace ReceiptReader.View
{
    public partial class MainPage : ContentPage
    {
        public MainPage(ItemsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
