using Picart.Models;
using Picart.Pages;
using Picart.Services.UserAppThemeSettingsService;
using System.Collections.ObjectModel;

namespace Picart.ViewModels
{
    internal partial class MainViewModel : ViewModelBase
    {
        private readonly IUserAppThemeSettingsService _userAppThemeSettingsService;

        private static Page MainPage => Application.Current!.Windows[0].Page!;

        public ObservableCollection<ProductGroup> ProductGroups { get; } =
        [
            new( "Owoce i warzywa", [new Product("jabłka"), new Product("banany"), new Product("pomidory"), new Product("ogórki"), new Product("ziemniaki")] ),
            new( "Jogurty", [new Product("skyr"), new Product("jogurt grecki"), new Product("jogurt naturalny"), new Product("jogurt owocowy")] ),
            new( "Sery", [new Product("ser żółty"), new Product("ser pleśniowy"), new Product("mozarella")] ),
            new( "Wędliny", [new Product("krakowska sucha"), new Product("kabanosy"), new Product("parówki")] )
        ];

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
            AddGroupCommand = new Command(async () => await AddGroupAsync());
            RemoveGroupCommand = new Command(() => ProductGroups.Remove(ProductGroups.Last()));
        }

        private async Task AddGroupAsync(string initialName = "")
        {
            string result = await MainPage.DisplayPromptAsync("New Group", "Enter the name of the new group:", initialValue: initialName);
            if (ProductGroups.Any(ProductGroups => ProductGroups.Name.Equals(result, StringComparison.OrdinalIgnoreCase)))
            {
                await MainPage.DisplayAlert("Error", "Group with this name already exists", "OK");
                await AddGroupAsync(result);
            }
            else if (string.IsNullOrWhiteSpace(result))
            {
                await MainPage.DisplayAlert("Error", "Group name cannot be empty", "OK");
                await AddGroupAsync();
            }
            else
            {
                ProductGroups.Add(new ProductGroup(result, []));
            }
        }

        private async void NavigateToProductGroup(ProductGroup productGroup)
        {
            if (MainPage is NavigationPage navigationPage)
            {
                await navigationPage.PushAsync(new ProductGroupPage(productGroup));
            }
        }
    }
}
