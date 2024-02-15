using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Services
{
    public class ProductService
    {
        private readonly ShopContext _context;

        public ProductService(ShopContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task UpdateProduct(Product product)
        {
            try
            {
                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var databaseValues = await entry.GetDatabaseValuesAsync();
                var databaseProduct = (Product)databaseValues.ToObject();

                product.Name = MergeValues(product.Name, databaseProduct.Name);
                product.Description = MergeValues(product.Description, databaseProduct.Description);
                product.Price = product.Price == 0 ? databaseProduct.Price : product.Price;
                product.AvailableQuantity = product.AvailableQuantity == 0 ? databaseProduct.AvailableQuantity : product.AvailableQuantity;

                product.RowVersion = databaseProduct.RowVersion;
                await _context.SaveChangesAsync();
            }
        }

        private string MergeValues(string newValue, string databaseValue)
        {
            return string.IsNullOrEmpty(newValue) ? databaseValue : newValue;
        }
    }
}
