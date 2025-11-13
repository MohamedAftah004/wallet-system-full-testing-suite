using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Wallet.Domain.Common;
using Wallet.Domain.Enums;

namespace Wallet.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FullName {  get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string PasswordHash { get; private set; }
        public UserStatus UserStatus { get; private set; }

        //Navigation Properties usr can have many wallts
        public ICollection<Wallet> Wallets { get; private set; } = new List<Wallet>();


        private User() { }

        //with required data
        public User(string fullName, string email, string phoneNumber, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Fullname is required" , nameof(fullName));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required", nameof(email));

            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone is required", nameof(phoneNumber));

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password is required", nameof(passwordHash));

            FullName = fullName;
            Email = email;
            PhoneNumber = phoneNumber;
            PasswordHash = passwordHash;
            UserStatus = UserStatus.PendingActivation;
        }


        //Updating FullName
        public void UpdateFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Full name cannot be empty", nameof(fullName));

            FullName = fullName;
            SetUpdatedAt();
        }

        //Updating PhoneNumber
        public void UpdatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number cannot be empty", nameof(phoneNumber));

            PhoneNumber = phoneNumber;
            SetUpdatedAt();
        }
        

        //Updating Password
        public void UpdatePassword(string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("Password cannot be empty", nameof(newPassword));

            PasswordHash = newPassword;
            SetUpdatedAt();
        }


        ///<summary>
        ///Activate User
        /// </summary>
        public void Activate()
        {
            UserStatus = UserStatus.Active;
            SetUpdatedAt();
        }

        ///<summary>
        ///Close User
        /// </summary>
        public void Close()
        {
            UserStatus = UserStatus.Closed;
            SetUpdatedAt();
        }

        ///<summary>
        ///Freeze User
        /// </summary>
        public void Freeze()
        {
            UserStatus = UserStatus.Frozen;
            SetUpdatedAt();
        }

        ///<summary>
        ///Disable User
        /// </summary>
        public void Disable()
        {
            UserStatus = UserStatus.Disabled;
            SetUpdatedAt();
        }


    }
}
