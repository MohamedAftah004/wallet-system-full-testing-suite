using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Domain.Enums
{
    public enum WalletStatus
    {
        PendingActivation = 0 ,
        Active = 1 ,
        Closed = 2 ,
        Frozen = 3 ,
        Disabled = 4
    };
}
