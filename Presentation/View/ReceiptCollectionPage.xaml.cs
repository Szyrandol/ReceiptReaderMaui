namespace ReceiptReader.View;

public partial class ReceiptCollectionPage : ContentPage
{
	public ReceiptCollectionPage(ReceiptCollectionViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}