using Picart.Models;

namespace Picart.Converters
{
    public interface IProductListConverter
    {
        Task<IEnumerable<ProductGroup>> ConvertAsync(string rawList, List<string> productGroups);
    }
}