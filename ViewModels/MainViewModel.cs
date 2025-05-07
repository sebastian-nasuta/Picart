using Picart.Converters;
using Picart.Models;
using Picart.Pages;
using Picart.Services.UserAppThemeSettingsService;
using System.Collections.ObjectModel;

namespace Picart.ViewModels
{
    internal partial class MainViewModel : ViewModelBase
    {
        private readonly IUserAppThemeSettingsService _userAppThemeSettingsService;
        private readonly IProductListConverter _productListConverter;

        private bool _isLoading;

        private static Page MainPage => Application.Current!.Windows[0].Page!;

        public ObservableCollection<ProductGroup> ProductGroups { get; private set; } =
        [
            /*
            new( "Owoce i warzywa", [new Product("jabłka"), new Product("banany"), new Product("pomidory"), new Product("ogórki"), new Product("ziemniaki")] ),
            new( "Jogurty", [new Product("skyr"), new Product("jogurt grecki"), new Product("jogurt naturalny"), new Product("jogurt owocowy")] ),
            new( "Sery", [new Product("ser żółty"), new Product("ser pleśniowy"), new Product("mozarella")] ),
            new( "Wędliny", [new Product("krakowska sucha"), new Product("kabanosy"), new Product("parówki")] )
            */
            new("Produkty zbożowe", []),
            new("Mięso", []),
            new("Warzywa", []),
            new("Nabiał", []),
            new("Owoce", []),
            new("Dziecięce", [])
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

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public int ProductsInitCount { get; set; }
        public int ProductsCount => ProductGroups.SelectMany(x => x.Products).Count();

        public Command AddProductGroupCommand { get; }
        public Command<ProductGroup> DeleteProductGroupCommand { get; }
        public Command GenerateListCommand { get; }
        public Command<ProductGroup> NavigateToProductGroupCommand { get; }

        public MainViewModel(IProductListConverter productListConverter, IUserAppThemeSettingsService userAppThemeSettingsService)
        {
            _productListConverter = productListConverter;
            _userAppThemeSettingsService = userAppThemeSettingsService;

            AddProductGroupCommand = new Command(async () => await AddProductGroupAsync());
            DeleteProductGroupCommand = new Command<ProductGroup>(DeleteProductGroup);
            GenerateListCommand = new Command(async () => await GenerateListAsync());
            NavigateToProductGroupCommand = new Command<ProductGroup>(NavigateToProductGroup);
        }

        private async Task AddProductGroupAsync(string initialName = "")
        {
            var title = "New Group";
            var result = await MainPage.DisplayPromptAsync(title, "Enter the name of the new group:", initialValue: initialName);
            if (ProductGroups.Any(ProductGroups => ProductGroups.Name.Equals(result, StringComparison.OrdinalIgnoreCase)))
            {
                await MainPage.DisplayAlert(title, "Group with this name already exists", "OK");
                await AddProductGroupAsync(result);
            }
            else if (!string.IsNullOrWhiteSpace(result))
            {
                ProductGroups.Add(new ProductGroup(result, []));
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

        private async Task GenerateListAsync()
        {
            try
            {
                var initRawList = @"makaron; ryż; pieluszki; kurczak; chusteczki; zbawki; klocki; pies; butelka; głośnik; wołowina; sałata; pomidory;
ser cheddar; jogurt grecki; mleko sojowe; tofu; chleb razowy; masło orzechowe; banany; jabłka; gruszki";

                initRawList = """
                    - makaron
                    - ryż 1kg
                    - pieluszki
                    - kurczak
                    - chusteczki - 2 pudełka
                    - zabawki
                    - klocki
                    - pies
                    - sok pomarańczowy - karton
                    - butelka
                    - głośnik
                    - wołowina 0.5kg
                    - sałata
                    - pomidory
                    - ser cheddar 300g
                    - jogurt grecki 3
                    - mleko sojowe
                    - tofu
                    - chleb razowy
                    - masło orzechowe
                    - banany
                    - jabłka 4
                    - gruszki 3
                    """;

                ProductsInitCount = initRawList.Split('-').Length - 3;
                OnPropertyChanged(nameof(ProductsInitCount));

                var rawList = await MainPage.DisplayPromptAsync("Generate List", "Enter the raw list:", initialValue: initRawList);
                if(string.IsNullOrWhiteSpace(rawList))
                {
                    return;
                }
                IsLoading = true;
                var productGroupNames = ProductGroups.Select(x => x.Name).ToList();
                var convertedProductGroups = await _productListConverter.ConvertAsync(rawList, productGroupNames);

                ProductGroups.Clear();
                foreach (var productGroup in convertedProductGroups)
                {
                    ProductGroups.Add(productGroup);
                }

                OnPropertyChanged(nameof(ProductsCount));
            }
            catch (Exception ex)
            {
                App.ShowExceptionAlert($"An error occurred while generating the list: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
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
