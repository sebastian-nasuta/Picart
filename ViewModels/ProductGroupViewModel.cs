using System.Collections.ObjectModel;
using Picart.Models;

namespace Picart.ViewModels
{
    internal partial class ProductGroupViewModel : ViewModelBase
    {
        public string GroupName { get; }
        public ObservableCollection<Product> Products { get; }

        public ProductGroupViewModel(ProductGroup productGroup)
        {
            GroupName = productGroup.Name;
            Products = new ObservableCollection<Product>(productGroup.Products);
        }
    }
}
