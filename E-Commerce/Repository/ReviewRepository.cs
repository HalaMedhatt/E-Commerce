using E_Commerce.IRepository;
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
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Edit(ProductReview item)
        {
            throw new NotImplementedException();
        }

        public List<ProductReview> GetAll()
        {
            return _context.ProductReviews.ToList();
        }

        public ProductReview GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
