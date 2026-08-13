using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MiniCrm.Application.Interfaces;
using MiniCrm.Application.Interfaces.Repositories;
using MiniCrm.Application.Interfaces.Services;
using MiniCrm.Application.Services;
using MiniCrm.Persistence.Context;
using MiniCrm.Persistence.Identity;
using MiniCrm.Persistence.Repositories;
using MiniCrm.Persistence.Services;

namespace MiniCrm.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        string connectionString)
    {
        // ============================================================
        // DATABASE
        // ============================================================

        services.AddDbContext<AppDbContext>(
            options =>
            {
                options.UseSqlServer(
                    connectionString);
            });


        // ============================================================
        // IDENTITY
        // ============================================================

        services
            .AddIdentityCore<ApplicationUser>(
                options =>
                {
                    options.User.RequireUniqueEmail =
                        true;

                    options.Password.RequiredLength =
                        8;

                    options.Password.RequireDigit =
                        true;

                    options.Password.RequireLowercase =
                        true;

                    options.Password.RequireUppercase =
                        true;

                    options.Password.RequireNonAlphanumeric =
                        false;

                    options.Lockout.AllowedForNewUsers =
                        true;

                    options.Lockout.MaxFailedAccessAttempts =
                        5;

                    options.Lockout.DefaultLockoutTimeSpan =
                        TimeSpan.FromMinutes(5);
                })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>();


        // ============================================================
        // UNIT OF WORK
        // ============================================================

        services.AddScoped<
            IUnitOfWork,
            UnitOfWork>();


        // ============================================================
        // REPOSITORIES
        // ============================================================

        services.AddScoped<
            ICustomerRepository,
            CustomerRepository>();

        services.AddScoped<
            ICustomerAddressRepository,
            CustomerAddressRepository>();

        services.AddScoped<
            ICategoryRepository,
            CategoryRepository>();

        services.AddScoped<
            IProductRepository,
            ProductRepository>();

        services.AddScoped<
            IStockMovementRepository,
            StockMovementRepository>();

        services.AddScoped<
            ICartRepository,
            CartRepository>();

        services.AddScoped<
            IOrderRepository,
            OrderRepository>();

        services.AddScoped<
            ITicketRepository,
            TicketRepository>();

        services.AddScoped<
            IComplaintRepository,
            ComplaintRepository>();

        services.AddScoped<
            ISupportConversationRepository,
            SupportConversationRepository>();

        services.AddScoped<
            IAfterSalesRequestRepository,
            AfterSalesRequestRepository>();

        services.AddScoped<
            IEmailLogRepository,
            EmailLogRepository>();


        // ============================================================
        // SERVICES
        // ============================================================

        services.AddScoped<
            IAuthService,
            AuthService>();

        services.AddScoped<
            ICategoryService,
            CategoryService>();

        services.AddScoped<
            IProductService,
            ProductService>();

        services.AddScoped<
            ICartService,
            CartService>();

        services.AddScoped<
            IOrderService,
            OrderService>();

        services.AddScoped<
            ICustomerProfileService,
            CustomerProfileService>();

        services.AddScoped<
            ICustomerAddressService,
            CustomerAddressService>();

        services.AddScoped<
            ITicketService,
            TicketService>();

        services.AddScoped<
            IComplaintService,
            ComplaintService>();

        services.AddScoped<
            ISupportConversationService,
            SupportConversationService>();

        services.AddScoped<
            IAfterSalesRequestService,
            AfterSalesRequestService>();

        return services;
    }
}