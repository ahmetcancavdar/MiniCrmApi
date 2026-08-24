using MiniCrm.Application.Interfaces;
using MiniCrm.Persistence.Context;

namespace MiniCrm.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(
        AppDbContext context)
    {
        _context = context;
    }

    // DbUpdateConcurrencyException burada yakalanmıyor; olduğu gibi
    // fırlatılmasına izin verilir ki GlobalExceptionHandler'daki
    // özel 409 (Conflict) eşlemesi devreye girsin. Daha önce burada
    // InvalidOperationException'a çevriliyordu, bu da her zaman 400
    // dönmesine ve 409 eşlemesinin hiç tetiklenmemesine yol açıyordu.
    public Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(
            cancellationToken);
    }
}