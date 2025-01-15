using Picart.Models;
using Picart.ViewModels;

namespace Picart.Pages;

public partial class ProductGroupPage : ContentPage
{
    public ProductGroupPage(ProductGroup productGroup)
    {
        InitializeComponent();
        BindingContext = new ProductGroupViewModel(productGroup);
    }
}
