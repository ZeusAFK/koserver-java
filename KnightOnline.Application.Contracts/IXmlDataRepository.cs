namespace KnightOnline.Application.Contracts.Infrastructure
{
    public interface IXmlDataRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        // Add other common data access methods as needed
    }
}
