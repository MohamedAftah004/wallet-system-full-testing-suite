using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Domain.Common;

namespace Wallet.Domain.Entities
{
    public class PaymentGatewayLog : BaseEntity
    {
        public Guid TransactionId { get; private set; }
        public string GatewayName { get; private set; }
        public string RequestPayload { get; private set; }
        public string ResponsePayload { get; private set; }
        public string StatusCode { get; private set; }

        public Transaction Transaction { get; private set; }

        public PaymentGatewayLog()
        {
            
        }


        public PaymentGatewayLog(Guid transactionId , string gatewayName, string requestPayload, string responsePayload, string statusCode)
        {
            if (transactionId == Guid.Empty)
                throw new ArgumentException("TransactionId is required", nameof(transactionId));

            TransactionId = transactionId;
            GatewayName = gatewayName ?? throw new ArgumentNullException(nameof(gatewayName));
            RequestPayload = requestPayload ?? "{}";
            ResponsePayload = responsePayload ?? "{}";
            StatusCode = statusCode ?? "Unknown";
        }

    }
}
