using Corolla_GUIMVVM_E120.Contants;
using Corolla_GUIMVVM_E120.Services.LoaderService;

using Windows.UI.Xaml.Controls;

namespace Corolla_GUIMVVM_E120.Services.DialogService
{
    public class DialogService : IDialogService
    {
        private readonly ILoaderService _loaderService;

        public DialogService(ILoaderService loaderService)
        {
            _loaderService = loaderService;
        }

        public void StatusNotification(string message, NotifyType type)
        {
            ContentDialog contentDialog = new ContentDialog();
            switch (type)
            {
                case NotifyType.Status:
                    contentDialog.Title = _loaderService.GetString(_loaderService.GetString("StatusApplicationDialog"));
                    break;
                case NotifyType.Information:
                    contentDialog.Title = _loaderService.GetString(_loaderService.GetString("InformationApplicationDialog"));
                    break;
                case NotifyType.Error:
                    contentDialog.Title = _loaderService.GetString(_loaderService.GetString("StatusApplicationDialog"));
                    break;
                default:
                    break;
            }
            contentDialog.Content = message;
            contentDialog.CloseButtonText = _loaderService.GetString("ButtonClose");
            _ = contentDialog.ShowAsync();
        }
    }

}
