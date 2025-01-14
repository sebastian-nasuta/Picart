using Picart.Services.UserAppThemeSettingsService;
using Picart.ViewModels;

namespace Picart
{
    public partial class MainPage : ContentPage
    {
        public MainPage(IUserAppThemeSettingsService userAppThemeSettingsService)
        {
            InitializeComponent();
            BindingContext = new MainViewModel(userAppThemeSettingsService);
        }
    }
}
