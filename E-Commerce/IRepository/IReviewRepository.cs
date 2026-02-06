using E_Commerce.Models;
using E_Commerce.Repository;

namespace E_Commerce.IRepository
{
    public interface IReviewRepository : IRepository<ProductReview>
    {
        public IEnumerable<ProductReview> GetReviewsByProductId(int productId);



    }
}
