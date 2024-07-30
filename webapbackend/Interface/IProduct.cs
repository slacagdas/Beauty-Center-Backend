using webapbackend.Models;

namespace webapbackend.Interface
{
    public interface IProduct
    {
        ICollection<Product> GetProducts();
        Product GetProduct(int id);
        bool ProductExists(int id);
        bool CreateProduct(Product product);
        bool UpdateProduct(Product product);
        bool DeleteProduct(Product product);
        bool Save();
    }
}
