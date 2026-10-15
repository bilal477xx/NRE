using System.Threading.Tasks;
using MsBox.Avalonia;

namespace NRE.Helpers;

public static class Dialogs
{
    public static Task ShowErrorAsync(string message) =>
        MessageBoxManager.GetMessageBoxStandard("Error", message).ShowAsync();

    public static Task ShowAsync(string title, string message) =>
        MessageBoxManager.GetMessageBoxStandard(title, message).ShowAsync();
}
