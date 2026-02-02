using E_Commerce.IRepository;
using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repository
{
    public class ProductVariantRepository : IProductVariantRepository
    {
        private readonly ECommerceDbContext _context;
        public ProductVariantRepository(ECommerceDbContext context)
        {

            _context = context;
        }
        public void Add(ProductVariant item)
        {
            _context.ProductVariants.Add(item);

        }

        public void Delete(int id)
        {
            var variant = _context.ProductVariants.Find(id);
            if (variant != null)
            {
                _context.ProductVariants.Remove(variant);
            }
        }

        public void Edit(ProductVariant item)
        {
            _context.ProductVariants.Update(item);
        }

        public List<ProductVariant> GetAll()
        {
            return _context.ProductVariants.Include(pv => pv.Product).ToList();
        }

        public ProductVariant GetById(int id)
        {
            return _context.ProductVariants
                .Include(pv => pv.Product)
                .FirstOrDefault(pv => pv.Id == id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
