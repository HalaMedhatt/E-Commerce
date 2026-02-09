using E_Commerce.IRepository;
using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ECommerceDbContext _context;
        public ProductRepository(ECommerceDbContext context)
        {
            _context = context;

        }
        public void Add(Product item)
        {
            _context.Products.Add(item);
        }

        public void Delete(int id)
        {
            _context.Products.Remove(GetById(id));
        }

        public void Edit(Product item)
        {
            _context.Products.Update(item);
        }

        public List<Product> GetAll()
        {
            return _context.Products
                .Include(Product => Product.Category)
                .Include(p => p.Variants)
                .Include(p => p.Images)
                .ToList();
        }

        public Product GetById(int id)
        {
            return _context.Products
                .Include(p => p.Reviews)
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .Include(p => p.Images)
                .FirstOrDefault(p => p.Id == id);


        }

        public void Save()
        {
            _context.SaveChanges();

        }
    }
}
