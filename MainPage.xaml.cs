using Picart.Services.UserAppThemeSettingsService;

namespace Picart
{
    public partial class MainPage : ContentPage
    {
        private readonly IUserAppThemeSettingsService _userAppThemeSettingsService;
        int count = 0;

        public MainPage(IUserAppThemeSettingsService userAppThemeSettingsService)
        {
            InitializeComponent();
            _userAppThemeSettingsService = userAppThemeSettingsService;
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private void ThemeToggler_Clicked(object sender, EventArgs e)
        {
            Dispatcher.Dispatch(_userAppThemeSettingsService.ToggleUserAppTheme);
        }
    }
}
