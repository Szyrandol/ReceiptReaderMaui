using ReceiptReader.ApplicationLayer.Interfaces;
using Microsoft.Maui.Controls;

namespace ReceiptReader.Infastructure.MauiServices;

public class DialogService : IDialogService
{
    public async Task<string?> PromptAsync(
        string title,
        string message,
        string accept = "OK",
        string cancel = "Cancel",
        string initialValue = "")
    {
        return await Shell.Current.DisplayPromptAsync(
            title,
            message,
            accept,
            cancel,
            keyboard: Keyboard.Default,
            initialValue: initialValue);
    }
    public async Task Alert(string title, string message, string cancel = "OK")
    {
        await Shell.Current.DisplayAlert(title, message, cancel);
    }

}
