using System.Collections.ObjectModel;
using System.Windows.Input;
using Picart.Models;

namespace Picart.ViewModels
{
    internal partial class ProductGroupViewModel : ViewModelBase
    {
        public string GroupName { get; }
        public ObservableCollection<Product> Products { get; }

        public Command<Product> DeleteProductCommand { get; }

        public ProductGroupViewModel(ProductGroup productGroup)
        {
            GroupName = productGroup.Name;
            Products = new ObservableCollection<Product>(productGroup.Products);
            DeleteProductCommand = new Command<Product>(DeleteProduct);
        }

        private void DeleteProduct(Product product) => Products.Remove(product);
    }
}
