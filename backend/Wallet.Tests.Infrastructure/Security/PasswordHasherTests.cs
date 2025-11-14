using Wallet.Infrastructure.Security;
using Xunit;

namespace Wallet.Tests.Infrastructure.Security
{
    public class PasswordHasherTests
    {
        private readonly PasswordHasher _passwordHasher;

        public PasswordHasherTests()
        {
            _passwordHasher = new PasswordHasher();
        }

        
        [Fact]
        public void Hash_Should_Return_Hashed_Password()
        {
            string password = "MyStrongPassword123";

            var hashed = _passwordHasher.Hash(password);

            Assert.False(string.IsNullOrWhiteSpace(hashed));
            Assert.NotEqual(password, hashed); 
            Assert.True(hashed.StartsWith("$2")); 
        }

        
        [Fact]
        public void Verify_Should_Return_True_For_Correct_Password()
        {
            string password = "Secret123";
            var hashed = _passwordHasher.Hash(password);

            var result = _passwordHasher.Verify(password, hashed);

            Assert.True(result);
        }

        [Fact]
        public void Verify_Should_Return_False_For_Wrong_Password()
        {
            string password = "CorrectPassword";
            var hashed = _passwordHasher.Hash(password);

            var result = _passwordHasher.Verify("WrongPassword", hashed);

            Assert.False(result);
        }


        [Fact]
        public void Hash_Should_Generate_Different_Hash_Each_Time()
        {
            string password = "test123";

            var hash1 = _passwordHasher.Hash(password);
            var hash2 = _passwordHasher.Hash(password);

            Assert.NotEqual(hash1, hash2);
        }
    }
}
