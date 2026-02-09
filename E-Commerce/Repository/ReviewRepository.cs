using E_Commerce.IRepository;
using Microsoft.EntityFrameworkCore;
using E_Commerce.Models;

namespace E_Commerce.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ECommerceDbContext _context;
        public ReviewRepository(ECommerceDbContext context)
        {
            _context = context;
        }
        public void Add(ProductReview item)
        {
            _context.ProductReviews.Add(item);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            _context.ProductReviews.Remove(GetById(id));
        }

        public void Edit(ProductReview item)
        {
            _context.ProductReviews.Update(item);
        }

        public List<ProductReview> GetAll()
        {
            return _context.ProductReviews
                .Include(p => p.Product)
                .Include(u => u.User)
                .ToList();
        }

        public ProductReview GetById(int id)
        {
            return _context.ProductReviews
                .Include(p => p.Product)
                .Include(p => p.User)
                .FirstOrDefault(p => p.Id == id);
        }
        public IEnumerable<ProductReview> GetReviewsByProductId(int productId)
        {
            return _context.ProductReviews
                .Include(r => r.Product)
                .Include(r => r.User)
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
