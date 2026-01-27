namespace E_Commerce.Repository
{
    public interface IRepository<T>
    {
        void Add(T item);
        T GetById(int id);
        List<T> GetAll();
        void Save();
        void Edit(T item);

    }
}
