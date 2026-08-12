using MiniCrmApi.Domain;

namespace MiniCrmApi.Repositories;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAllActiveAsync();
    Task<Customer?> GetActiveByIdAsync(int id);
    Task AddAsync(Customer customer);
    void Update(Customer customer);
    Task SaveChangesAsync();
}