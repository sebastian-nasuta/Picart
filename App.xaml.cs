using Picart.Services.UserAppThemeSettingsService;

namespace Picart
{
    public partial class App : Application
    {
        private readonly IUserAppThemeSettingsService _userAppThemeSettingsService;

        public App(IUserAppThemeSettingsService userAppThemeSettingsService)
        {
            InitializeComponent();
            _userAppThemeSettingsService = userAppThemeSettingsService;
            _userAppThemeSettingsService.ReloadUserAppTheme();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var mainPage = new MainPage(_userAppThemeSettingsService);
            NavigationPage.SetHasNavigationBar(mainPage, false);
            return new Window(new NavigationPage(mainPage));
        }
    }
}