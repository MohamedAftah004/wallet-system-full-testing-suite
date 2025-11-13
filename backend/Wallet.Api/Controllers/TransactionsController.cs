using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Transactions.History.Queries.GetAllTransactionsInfo;
using Wallet.Application.Transactions.History.Queries.GetRecentTransactions;
using Wallet.Application.Transactions.History.Queries.GetTransactions;
using Wallet.Application.Transactions.Payments.Commands.MakePayment;
using Wallet.Application.Transactions.Payments.Queries.GetWalletBalance;
using Wallet.Application.Transactions.Refunds.Commands.RefundTransaction;
using Wallet.Application.Transactions.Refunds.Queries.GetRefundsByWalletId;
using Wallet.Application.Transactions.TopUp.Commands.TopUpWallet;
using Wallet.Application.Transactions.TopUp.Queries.GetTransactionById;

namespace Wallet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {

        private readonly IMediator _mediator;

        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Processes a wallet top-up request.
        /// </summary>
        /// <param name="command">The command containing the details of the wallet top-up operation to perform. Must not be null.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the top-up operation.</param>
        /// <returns></returns>
        [HttpPost("topup")]
        public async Task<IActionResult> TopUp([FromBody] TopUpWalletCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }


        /// <summary>
        /// Retrieves detailed information for a specific transaction by its unique id 
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("transaction-info/{transactionId:guid}")]
        public async Task<IActionResult> GetTransactionById(Guid transactionId, CancellationToken cancellationToken)
        {
            var query = new GetTransactionByIdQuery(transactionId);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }


        /// <summary>
        /// Processes a wallet payment reqoest.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("payment")]
        public async Task<IActionResult> MakePayment([FromBody] MakePaymentCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new MakePaymentCommand(command.WalletId, command.Amount, command.Description), cancellationToken);
            return Ok(result);
        }


        /// <summary>
        /// Retrieves the current balance of a specific wallet byy it,s id
        /// </summary>
        /// <param name="walletId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{walletId}/balance")]
        public async Task<IActionResult> GetBalance(Guid walletId, CancellationToken cancellationToken)
        {
            var query = new GetWalletBalanceQuery(walletId);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }


        /// <summary>
        /// Processes a refund for the specified transaction.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("refund/{transactionId:guid}")]
        public async Task<IActionResult> RefundTransaction(Guid transactionId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new RefundTransactionCommand(transactionId), cancellationToken);
            return Ok(new { Message = "Refund processed successfully." });
        }


        /// <summary>
        /// Retrieves all the refunds processed of a specific wallet
        /// </summary>
        /// <param name="walletId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("wallets/{walletId}/refunds")]
        public async Task<IActionResult> GetRefundsByWalletId(Guid walletId, CancellationToken cancellationToken)
        {
            var refunds = await _mediator.Send(new GetRefundsByWalletIdQuery(walletId), cancellationToken);
            return Ok(refunds);
        }



        /// <summary>
        /// Retrieves a paginated list of transactions for a specific wallet, with optional filtering by type, status, and date range.
        /// </summary>
        /// <param name="walletId">The unique identifier of the wallet.</param>
        /// <param name="type">Transaction type (TopUp, Payment, Refund).</param>
        /// <param name="status">Transaction status (Pending, Completed, Failed).</param>
        /// <param name="fromDate">Start date filter.</param>
        /// <param name="toDate">End date filter.</param>
        /// <param name="page">Page number (default = 1).</param>
        /// <param name="size">Page size (default = 20).</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A paginated list of transactions.</returns>
        [HttpGet("wallets/{walletId}/transactions")]
        public async Task<IActionResult> GetTransactions(
            Guid walletId,
            [FromQuery] string? type,
            [FromQuery] string? status,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] int page = 1,
            [FromQuery] int size = 20,
            CancellationToken cancellationToken = default
            )
        {
            var query = new GetTransactionsQuery(walletId, type, status, fromDate, toDate, page, size );
            var result = await _mediator.Send(query , cancellationToken);
            return Ok(result);
        }


        /// <summary>
        /// Retrieves all transactions (for admin use).
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllTransactions(
            [FromQuery] string? type,
            [FromQuery] string? status,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] int page = 1,
            [FromQuery] int size = 20)
        {
            var query = new GetAllTransactionsInfoQuery(type, status, fromDate, toDate, page, size);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        /// <summary>
        /// Retrieves the most recent transactions (optionally for a specific wallet).
        /// </summary>
        [HttpGet("recent")]
        public async Task<IActionResult> GetRecentTransactions(
            [FromQuery] Guid? walletId,
            [FromQuery] int count = 5)
        {
            var query = new GetRecentTransactionsQuery(walletId, count);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }

}
