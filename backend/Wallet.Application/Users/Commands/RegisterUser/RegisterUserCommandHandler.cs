using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Common.Exceptions;
using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Entities;

namespace Wallet.Application.Users.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserCommandHandler(IUserRepository userRepository , IPasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
        }

        public async Task <Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {

            //check -> [email,phone] -> not exists
            if (await _userRepository.ExsistByEmailAsync(request.Email, cancellationToken))
                throw new DuplicateEmailException(request.Email);

            if(await _userRepository.ExsistByPhoneAsync(request.PhoneNumber, cancellationToken))
                throw new DuplicatePhoneException(request.PhoneNumber);


            var hashedPassword = _passwordHasher.Hash(request.Password);


            var user = new User(
                request.FullName,
                request.Email,
                request.PhoneNumber,
                hashedPassword
                );


            await _userRepository.AddAsync(user , cancellationToken);
            return user.Id;
        }
    }
}
