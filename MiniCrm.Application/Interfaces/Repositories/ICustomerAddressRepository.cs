using MiniCrm.Domain.Entities;

namespace MiniCrm.Application.Interfaces.Repositories;

public interface ICustomerAddressRepository
{
    Task<List<CustomerAddress>> GetByCustomerIdAsync(
        int customerId,
        CancellationToken cancellationToken = default);

    Task<CustomerAddress?> GetByIdAsync(
        int addressId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        CustomerAddress address,
        CancellationToken cancellationToken = default);

    void Remove(
        CustomerAddress address);
}