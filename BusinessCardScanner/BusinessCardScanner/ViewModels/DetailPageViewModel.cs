using System;
using System.Threading.Tasks;
using BusinessCardScanner.Cognitive.Entities;
using Prism.Commands;
using Prism.Navigation;

namespace BusinessCardScanner.ViewModels
{
    public class DetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IUserDataWrapper _userDataWrapper;
        public DetailPageViewModel(INavigationService navigationService, IUserDataWrapper userDataWrapper) : base(navigationService)
        {
            _navigationService = navigationService;
            _userDataWrapper = userDataWrapper;
            SaveCommand = new DelegateCommand(async () => await SaveCommandHandler());
            CancelCommand = new DelegateCommand(async () => await CancelCommandHandler());
        }

        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        public ContactCard ContactDetails { get; set; } = new ContactCard();

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            ContactDetails = parameters["CardDetails"] as ContactCard;
            RaisePropertyChanged(nameof(ContactDetails));
        }

        private async Task CancelCommandHandler() => await _navigationService.GoBackAsync();

        private async Task SaveCommandHandler()
        {
            _userDataWrapper.Insert(ContactDetails);
            await _navigationService.GoBackAsync();
        }
    }
}