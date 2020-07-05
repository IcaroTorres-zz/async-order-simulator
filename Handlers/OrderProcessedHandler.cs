using MediatR;
using Microsoft.AspNetCore.SignalR;
using OrderSimulator.Extensions;
using OrderSimulator.Hubs;
using OrderSimulator.Models;
using System.Threading;
using System.Threading.Tasks;

namespace OrderSimulator.Handlers
{
    public class OrderProcessedHandler : IPipelineBehavior<Order, Unit>
    {
        private readonly IHubContext<OrderingHub> _hubContext;

        public OrderProcessedHandler(IHubContext<OrderingHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task<Unit> Handle(Order request, CancellationToken cancellationToken, RequestHandlerDelegate<Unit> next)
        {
            await Task.Delay(750).ConfigureAwait(false);
            request.ProcessedOrFailed();
            await _hubContext.Clients.All.SendAsync($"order-{request.Status.Name()}", request);
            return Unit.Value;
        }
    }
}