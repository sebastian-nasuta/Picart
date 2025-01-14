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
            return new Window(new MainPage(_userAppThemeSettingsService));
        }
    }
}