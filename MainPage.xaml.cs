using Picart.Converters;
using Picart.Services.UserAppThemeSettingsService;
using Picart.ViewModels;

namespace Picart
{
    public partial class MainPage : ContentPage
    {
        public MainPage(IProductListConverter productListConverter, IUserAppThemeSettingsService userAppThemeSettingsService)
        {
            InitializeComponent();
            BindingContext = new MainViewModel(productListConverter, userAppThemeSettingsService);
        }
    }
}
