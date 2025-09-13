namespace ReceiptReader.View
{
    public partial class NewReceiptPage : ContentPage
    {
        public NewReceiptPage(ItemsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
