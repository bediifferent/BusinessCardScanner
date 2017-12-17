using System;
using System.Collections.ObjectModel;
using Prism.Navigation;
using System.Threading.Tasks;
using BusinessCardScanner.Cognitive;
using BusinessCardScanner.Cognitive.Entities;
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
        private readonly IUserDataWrapper _userDataWrapper;
        private readonly INavigationService _navigationService;
        private ObservableCollection<ContactCard> _contacts;

        public MainPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IUserDataWrapper userDataWrapper) : base(navigationService)
        {
            _pageDialogService = pageDialogService;
            _userDataWrapper = userDataWrapper;
            _navigationService = navigationService;
            TakePhotoCommand = new DelegateCommand(async () => await TakePhotoCommandHandler());
        }

        public ObservableCollection<ContactCard> Contacts
        {
            get { return _contacts; }
            set
            {
                _contacts = value;
                RaisePropertyChanged();
            }
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            Title = "Contacts";
            Contacts = new ObservableCollection<ContactCard>(_userDataWrapper.LoadAll<ContactCard>());
        }

        public DelegateCommand TakePhotoCommand { get; set; }

        private async Task TakePhotoCommandHandler()
        {
            try
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
                {
                    return;
                }

                var stream = DependencyService.Get<IDeviceInfoService>().GetFileStream(file);

                var card = await OcrReader.ReadBusinessCard(stream);
                var parameters = new NavigationParameters
                {
                    { "CardDetails", card }
                };
                await _navigationService.NavigateAsync("DetailPage", parameters);
            }
            catch (Exception e)
            {
                await _pageDialogService.DisplayAlertAsync("Error", e.Message, "Cancel");
            }
        }
    }
}
