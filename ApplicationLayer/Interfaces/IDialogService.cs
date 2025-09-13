namespace ReceiptReader.ApplicationLayer.Interfaces;

public interface IDialogService
{
    Task<string?> PromptAsync(string title, string message, string accept = "OK",
                              string cancel = "Cancel", string initialValue = "");
    Task Alert(string title, string message, string cancel = "OK");
}
