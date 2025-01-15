using System.Collections.ObjectModel;
using Picart.Models;

namespace Picart.ViewModels
{
    internal partial class ProductGroupViewModel(ProductGroup productGroup) : ViewModelBase
    {
        public string GroupName { get; } = productGroup.Name;
        public ObservableCollection<Product> Products { get; } = new ObservableCollection<Product>(productGroup.Products);
    }
}
