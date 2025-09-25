using Microsoft.Extensions.Logging;
using ReceiptReader.ApplicationLayer.Interfaces;
using ReceiptReader.Domain.Repositories;
using ReceiptReader.Infastructure.Persistence;
using ReceiptReader.Infastructure.MauiServices;
using ReceiptReader.ApplicationLayer.Services;
using ReceiptReader.Infastructure.Services;

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
            builder.Services.AddTransient<ReceiptViewModel>();
            builder.Services.AddTransient<ReceiptPage>();

            //receipts
            builder.Services.AddSingleton<ReceiptService>();
            builder.Services.AddSingleton<ReceiptCollectionPage>();
            builder.Services.AddSingleton<ReceiptCollectionViewModel>();

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
