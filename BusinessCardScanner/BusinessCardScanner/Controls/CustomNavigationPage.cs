using Prism.Navigation;
using Xamarin.Forms;

namespace BusinessCardScanner.Controls
{
    public class CustomNavigationPage : NavigationPage, INavigationPageOptions
    {
        public bool ClearNavigationStackOnNavigation => false;
    }
}