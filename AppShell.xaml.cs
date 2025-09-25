namespace ReceiptReader
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(ReceiptPage), typeof(ReceiptPage));
        }
    }
}
