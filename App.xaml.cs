using Picart.Converters;
using Picart.Services.UserAppThemeSettingsService;

namespace Picart
{
    public partial class App : Application
    {
        private readonly IProductListConverter _productListConverter;
        private readonly IUserAppThemeSettingsService _userAppThemeSettingsService;

        public App(IProductListConverter productListConverter, IUserAppThemeSettingsService userAppThemeSettingsService)
        {
            InitializeComponent();

            _productListConverter = productListConverter;
            _userAppThemeSettingsService = userAppThemeSettingsService;
            
            _userAppThemeSettingsService.ReloadUserAppTheme();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var mainPage = new MainPage(_productListConverter, _userAppThemeSettingsService);
            NavigationPage.SetHasNavigationBar(mainPage, false);
            return new Window(new NavigationPage(mainPage));
        }

        public static void ShowExceptionAlert(string message, string title = "Error")
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                var mainPage = Current?.Windows[0].Page;
                if (mainPage is not null)
                {
                    await mainPage.DisplayAlert(title, message, "OK");
                }
            });
        }
    }
}