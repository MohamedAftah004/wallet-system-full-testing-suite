using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Wallet.Tests.Domain.Wallets
{
    public class WalletTests
    {

        //constructor test
        [Fact]
        public void Constructor_ShouldInitializeWallet_WithZeroBalance_AndPendingStatus()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var currency = new Currency("USD");
            //Act

            //Assert
        }




    }
}
