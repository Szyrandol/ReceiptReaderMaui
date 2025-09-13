using Microsoft.Extensions.Logging;
using ReceiptReader.ApplicationLayer.Interfaces;
using ReceiptReader.Domain.Repositories;
using ReceiptReader.Infastructure.Persistence;
using ReceiptReader.Infastructure.MauiServices;

namespace ReceiptReader
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            //items
            builder.Services.AddSingleton<OcrService>();
            builder.Services.AddTransient<ItemsViewModel>();
            builder.Services.AddTransient<NewReceiptPage>();

            //receipts
            builder.Services.AddSingleton<ReceiptService>();
            builder.Services.AddSingleton<ReceiptCollectionPage>();
            builder.Services.AddSingleton<ReceiptsViewModel>();

            //DI
            builder.Services.AddSingleton<IReceiptRepository, JsonReceiptRepository>();
            builder.Services.AddSingleton<IDialogService, DialogService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
