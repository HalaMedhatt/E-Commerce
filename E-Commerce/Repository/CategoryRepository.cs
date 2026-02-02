using E_Commerce.IRepository;
using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ECommerceDbContext _context;
        public CategoryRepository(ECommerceDbContext context)
        {
            _context = context;

        }
        public void Add(Category item)
        {
            _context.Categories.Add(item);
        }

        public void Delete(int id)
        {
            _context.Categories.Remove(GetById(id));
        }

        public void Edit(Category item)
        {
            _context.Categories.Update(item);
        }

        public List<Category> GetAll()
        {
            return _context.Categories
                .Include(c => c.Products)
                .ToList();
        }

        public Category GetById(int id)
        {
            return _context.Categories
                .Include(c => c.Products)
                
                .FirstOrDefault(c => c.Id == id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
