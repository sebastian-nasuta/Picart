namespace Picart.Models
{
    public class ProductGroup(string name, Product[] products)
    {
        public string Name { get; } = name;
        public Product[] Products { get; } = products;
    }
}
