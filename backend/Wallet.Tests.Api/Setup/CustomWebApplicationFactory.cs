using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Wallet.Application.Security.Commands.Login;
using MediatR;
using Wallet.Api;

namespace Wallet.Tests.Api.Setup
{
    public class CustomWebApplicationFactory
        : WebApplicationFactory<Program>
    {
        public Mock<IMediator> MediatorMock { get; } = new();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove real IMediator
                var mediatorDescriptor = services
                    .SingleOrDefault(s => s.ServiceType == typeof(IMediator));

                if (mediatorDescriptor != null)
                    services.Remove(mediatorDescriptor);

                // Register mocked mediator
                services.AddSingleton<IMediator>(MediatorMock.Object);
            });
        }
    }
}
