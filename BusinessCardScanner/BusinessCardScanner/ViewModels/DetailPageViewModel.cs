using BusinessCardScanner.Cognitive.Entities;
using Prism.Navigation;

namespace BusinessCardScanner.ViewModels
{
    public class DetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        public DetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
        }

        public ContactCard ContactDetails { get; set; } = new ContactCard();

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            ContactDetails = parameters["CardDetails"] as ContactCard;
            RaisePropertyChanged(nameof(ContactDetails));
        }
    }
}