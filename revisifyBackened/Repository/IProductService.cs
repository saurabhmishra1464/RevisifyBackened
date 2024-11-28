using revisifyBackened.Models;

namespace revisifyBackened.Repository
{
    public interface IProductService
    {
        public IEnumerable<Product> GetProducts();
        public Product PostProduct(Product product);
    }
}
