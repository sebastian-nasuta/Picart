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
            new( "Wędliny", [new Product("krakowska sucha"), new Product("kabanosy"), new Product("parówki")] ),
            new( "1", []),
            new( "2", []),
            new( "3", []),
            new( "4", []),
            new( "5", []),
            new( "6", []),
            new( "7", []),
            new( "8", []),
            new( "9", []),
            new( "10", [])
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
        public Command<ProductGroup> DeleteProductGroupCommand { get; }
        public Command AddGroupCommand { get; }

        public MainViewModel(IUserAppThemeSettingsService userAppThemeSettingsService)
        {
            _userAppThemeSettingsService = userAppThemeSettingsService;
            NavigateToProductGroupCommand = new Command<ProductGroup>(NavigateToProductGroup);
            DeleteProductGroupCommand = new Command<ProductGroup>(DeleteProductGroup);
            AddGroupCommand = new Command(async () => await AddGroupAsync());
        }

        private async Task AddGroupAsync(string initialName = "")
        {
            var title = "New Group";
            var result = await MainPage.DisplayPromptAsync(title, "Enter the name of the new group:", initialValue: initialName);
            if (ProductGroups.Any(ProductGroups => ProductGroups.Name.Equals(result, StringComparison.OrdinalIgnoreCase)))
            {
                await MainPage.DisplayAlert(title, "Group with this name already exists", "OK");
                await AddGroupAsync(result);
            }
            else if (!string.IsNullOrWhiteSpace(result))
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

        private async void DeleteProductGroup(ProductGroup productGroup)
        {
            bool result = await MainPage.DisplayAlert("Delete Group", $"Are you sure you want to delete the group '{productGroup.Name}'?", "Yes", "No");
            if (result)
            {
                ProductGroups.Remove(productGroup);
            }
        }
    }
}
