using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessCardScanner.Controls;
using BusinessCardScanner.ViewModels;
using BusinessCardScanner.Views;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Prism.Unity;
using Xamarin.Forms;

namespace BusinessCardScanner
{
	public partial class App 
	{
		public App (IPlatformInitializer initializer) : base(initializer)
		{
			InitializeComponent();
		    ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(Container));
        }
        protected override async void OnInitialized()
        {
            await NavigationService.NavigateAsync("/Slider/NavigationPage/Home");
        }

        protected override void RegisterTypes()
	    {
	        Container.RegisterTypeForNavigation<CustomNavigationPage>("NavigationPage");
	        Container.RegisterTypeForNavigation<SliderView, SliderViewModel>("Slider");
            Container.RegisterTypeForNavigation<HomeView,HomeViewModel>("Home");
        }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}
	    protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
