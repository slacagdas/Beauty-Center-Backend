using webapbackend.Data;
using webapbackend.Interface;
using webapbackend.Models;

namespace webapbackend.Repository
{
    public class ProductRepository : IProduct
    {
        private DataContext _context;
        public ProductRepository(DataContext context)
        {
            _context = context;
        }
        public bool ProductExists(int id)
        {
            return _context.Products.Any(c => c.Id == id);
        }

        public bool CreateProduct(Product product)
        {
            _context.Add(product);
            return Save();
        }

        public bool DeleteProduct(Product product)
        {
            _context.Remove(product);
            return Save();
        }

        public ICollection<Product> GetProducts()
        {
            return _context.Products.ToList();
        }

        public Product GetProduct(int id)
        {
            return _context.Products.Where(e => e.Id == id).FirstOrDefault();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateProduct(Product product)
        {
            _context.Update(product);
            return Save();
        }
    }
}
