using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wallet.Api.DTOs.Wallets;
using Wallet.Application.Users.Commands.RegisterUser;
using Wallet.Application.Wallets.Commands.ActivateWallet;
using Wallet.Application.Wallets.Commands.CloseWallet;
using Wallet.Application.Wallets.Commands.CreateWallet;
using Wallet.Application.Wallets.Commands.FreezeWallet;
using Wallet.Application.Wallets.Queries.GetUserWallets;
using Wallet.Application.Wallets.Queries.GetWalletInfoById;

namespace Wallet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletsController : ControllerBase
    {

        private readonly IMediator _mediator;

        public WalletsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        /// <summary>
        /// Creates a new wallet for a specific user.
        /// </summary>
        /// <param name="dto">The wallet creation details.</param>
        /// <returns>Information about the newly created wallet.</returns>
        [HttpPost("create")]
        [ProducesResponseType(typeof(CreateWalletResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateWalletRequestDto dto, CancellationToken cancellationToken)
        {
            var command = new CreateWalletCommand(dto.UserId, dto.CurrencyCode);
            var result = await _mediator.Send(command, cancellationToken);
            

            return CreatedAtAction(nameof(GetById), new { walletId = result.WalletId }, result);
        }


        /// <summary>
        /// Activate wallet using walletId
        /// </summary>
        /// <param name="walletId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{walletId:guid}/activate")]
        public async Task<IActionResult> ActivateWallet(Guid walletId, CancellationToken cancellationToken)
        {

          await _mediator.Send(new ActivateWalletCommand(walletId) , cancellationToken);

            return NoContent();
        }


        /// <summary>
        /// Close wallet using walletid
        /// </summary>
        /// <param name="walletId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{walletId:guid}/close")]
        public async Task<IActionResult> CloseWallet(Guid walletId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new CloseWalletCommand(walletId), cancellationToken);
            return NoContent();
        }



        /// <summary>
        /// Freeze a wallet (temporarily suspend transactions).
        /// </summary>
        /// <param name="walletId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{walletId:guid}/freeze")]
        public async Task<IActionResult> FreezeWallet(Guid walletId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new FreezeWalletCommand(walletId), cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Get wallet info by ID (used by CreatedAtAction)
        /// </summary>
        [HttpGet("{walletId:guid}")]
        public async Task<IActionResult> GetById(Guid walletId, CancellationToken cancellationToken)
        {

            var query = new GetWalletInfoByIdQuery(walletId);
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Get all wallets that belong to a specific user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetUserWallets(Guid userId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetUserWalletsQuery(userId), cancellationToken);
            return Ok(result);
        }

    }
}
