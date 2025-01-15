using Picart.Models;
using Picart.Pages;
using Picart.Services.UserAppThemeSettingsService;
using System.Collections.ObjectModel;

namespace Picart.ViewModels
{
    internal partial class MainViewModel : ViewModelBase
    {
        private readonly IUserAppThemeSettingsService _userAppThemeSettingsService;

        public ObservableCollection<ProductGroup> ProductGroups { get; } = new()
            {
                new( "Owoce i warzywa", new[] { new Product("jabłka"), new Product("banany"), new Product("pomidory"), new Product("ogórki"), new Product("ziemniaki") } ),
                new( "Jogurty", new[] { new Product("skyr"), new Product("jogurt grecki"), new Product("jogurt naturalny"), new Product("jogurt owocowy") } ),
                new( "Sery", new[] { new Product("ser żółty"), new Product("ser pleśniowy"), new Product("mozarella") } ),
                new( "Wędliny", new[] { new Product("krakowska sucha"), new Product("kabanosy"), new Product("parówki") } )
            };

        public bool IsDarkTheme
        {
            get => Application.Current?.RequestedTheme == AppTheme.Dark;
            set
            {
                _userAppThemeSettingsService.SetUserAppTheme(value ? AppTheme.Dark : AppTheme.Light);
                OnPropertyChanged();
            }
        }

        public Command<ProductGroup> NavigateToProductGroupCommand { get; }
        public Command AddGroupCommand { get; }
        public Command RemoveGroupCommand { get; }

        public MainViewModel(IUserAppThemeSettingsService userAppThemeSettingsService)
        {
            _userAppThemeSettingsService = userAppThemeSettingsService;
            NavigateToProductGroupCommand = new Command<ProductGroup>(NavigateToProductGroup);
            AddGroupCommand = new Command(() => ProductGroups.Add(new($"group_{DateTime.UtcNow.Ticks}", new[] { new Product($"product_{DateTime.UtcNow.Ticks}") })));
            RemoveGroupCommand = new Command(() => ProductGroups.Remove(ProductGroups.Last()));
        }

        private async void NavigateToProductGroup(ProductGroup productGroup)
        {
            if (Application.Current?.Windows[0].Page is NavigationPage navigationPage)
            {
                await navigationPage.PushAsync(new ProductGroupPage(productGroup));
            }
        }
    }
}
