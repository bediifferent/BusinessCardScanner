using System;
using Prism.Navigation;
using System.Threading.Tasks;
using BusinessCardScanner.Cognitive;
using BusinessCardScanner.Services.Interfaces;
using Plugin.Media;
using Prism.Commands;
using Prism.Services;
using DependencyService = Xamarin.Forms.DependencyService;

namespace BusinessCardScanner.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService _pageDialogService;
        public MainPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base (navigationService)
        {
            _pageDialogService = pageDialogService;
            Title = "Main Page";
            TakePhotoCommand = new DelegateCommand(async ()=> await TakePhotoCommandHandler());
        }

        public DelegateCommand TakePhotoCommand { get; set; }

        private async Task TakePhotoCommandHandler()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await _pageDialogService.DisplayAlertAsync("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null)
                return;

            await _pageDialogService.DisplayAlertAsync("File Location", file.Path, "OK");

            var stream = DependencyService.Get<IDeviceInfoService>().GetFileStream(file);
            try
            {
                var abc = await OcrReader.ReadBusinessCard(stream);
                await _pageDialogService.DisplayAlertAsync("Error", abc.Name, "Ok", "Cancel");
                
            }
            catch (Exception e)
            {
                await _pageDialogService.DisplayAlertAsync("Error", e.Message, "Ok", "Cancel");
            }
            
            //image.Source = ImageSource.FromStream(() =>
            //{
            //    var stream = file.GetStream();
            //    file.Dispose();
            //    return stream;
            //});
        }
    }
}
