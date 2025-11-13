using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Wallet.Application.Common.Exceptions;
using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Entities;

namespace Wallet.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(User user, CancellationToken cancelationToken = default)
        {
            await _dbContext.Users.AddAsync(user, cancelationToken);
            await _dbContext.SaveChangesAsync(cancelationToken);

        }

        public IQueryable<User> Query() => _dbContext.Users.AsQueryable();

        public async Task ActivateAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null)
                throw new EntityNotFoundException(EntityType.User, userId.ToString());

            user.Activate();
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
                    
        }


        public async Task CloseAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null)
                throw new EntityNotFoundException(EntityType.User, userId.ToString());

            user.Close();
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DisableAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null)
                throw new EntityNotFoundException(EntityType.User, userId.ToString());

            user.Disable();
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExsistByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<bool> ExsistByPhoneAsync(string phone, CancellationToken camcellationToken = default)
        {
            return await _dbContext.Users.AnyAsync(u => u.PhoneNumber == phone, camcellationToken);
        }

        public async Task FreezeAsync(Guid userId, CancellationToken cancellationToken = default)
        {

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null)
                throw new EntityNotFoundException(EntityType.User, userId.ToString());

            user.Freeze();
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users
                           .AsNoTracking()
                           .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        }

        public async Task<User?> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users
               .AsNoTracking()
               .FirstOrDefaultAsync(u => u.PhoneNumber == phone, cancellationToken);
        }
    }
}
