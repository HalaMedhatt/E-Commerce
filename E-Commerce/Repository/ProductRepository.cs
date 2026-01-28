using E_Commerce.IRepository;
using E_Commerce.Models;

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

        public void Edit(Product item)
        {
            _context.Products.Update(item);
        }

        public  List<Product> GetAll()
        {
            return _context.Products.ToList();
        }

        public Product GetById(int id)
        {
            return _context.Products.Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();

        }
    }
}
