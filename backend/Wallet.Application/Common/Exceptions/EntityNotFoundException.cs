using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.Common.Exceptions
{

    public enum EntityType
    {
        User = 1,
        Admin = 2,
        Wallet = 3,
        Transaction = 4,
    }


    /// <summary>
    /// Exception thrown when a specific entity cannot be found in the data store.
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        public EntityType EntityType { get; }
        public string Key { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class 
        /// with a specific entity type and unique identifier.
        /// </summary>
        /// <param name="entityType">The type of entity that was not found (e.g., User, Wallet).</param>
        /// <param name="key">The unique identifier or key associated with the missing entity.</param>
        public EntityNotFoundException(EntityType entityType, string key)
                   : base($"{entityType} with identifier '{key}' was not found.")
        {
            EntityType = entityType;
            Key = key;
        }
    }
}
