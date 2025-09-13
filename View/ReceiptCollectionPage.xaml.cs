namespace ReceiptReader.View;

public partial class ReceiptCollectionPage : ContentPage
{
	public ReceiptCollectionPage(ReceiptsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}