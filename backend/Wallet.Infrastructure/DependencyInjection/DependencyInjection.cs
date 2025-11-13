using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Common.Interfaces;
using Wallet.Infrastructure.Persistence;
using Wallet.Infrastructure.Persistence.Repositories;
using Wallet.Infrastructure.Persistence.Repositories;
using Wallet.Infrastructure.Security;

namespace Wallet.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddInftrastructure(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(option =>
            option.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            //repo
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IWalletRepository , WalletRepository>();
            services.AddScoped<ITransactionRepository , TransactionRepository>();
            //security
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();

            return services;
        }

    }
}
