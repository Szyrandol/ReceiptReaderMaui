namespace ReceiptReader.View
{
    public partial class ReceiptPage : ContentPage
    {
        public ReceiptPage(ReceiptViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
