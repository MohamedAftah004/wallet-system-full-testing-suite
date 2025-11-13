using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Domain.Entities;

namespace Wallet.Application.Common.Interfaces
{
    public interface IUserRepository
    {


        //register
        Task AddAsync(User user, CancellationToken cancelationToken = default);


        //get user info by
        //id
        Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);
        //phone
        Task<User?> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default);
        //email
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        IQueryable<User> Query();

        //status operations
        //activate
        Task ActivateAsync(Guid userId, CancellationToken cancellationToken = default);
        //freeze
        Task FreezeAsync(Guid userId , CancellationToken cancellationToken = default);
        //disable
        Task DisableAsync(Guid userId, CancellationToken cancellationToken = default);
        //close
        Task CloseAsync(Guid userId, CancellationToken cancellationToken = default);


        //checks by
        //email
        Task<bool> ExsistByEmailAsync(string email, CancellationToken camcellationToken = default);
        //phone
        Task<bool> ExsistByPhoneAsync(string phone, CancellationToken camcellationToken = default);

    }
}
