using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.Common.Interfaces
{
    public interface IPasswordHasher
    {

        /// <summary>
        /// hashing password before storing it in database 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        string Hash(string password);



        /// <summary>
        /// verfiy password before against stored hash
        /// </summary>
        bool Verify(string password , string passwordHash);

    }
}
