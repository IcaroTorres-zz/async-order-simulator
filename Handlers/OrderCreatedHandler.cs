using MediatR;
using Microsoft.AspNetCore.SignalR;
using OrderSimulator.Hubs;
using OrderSimulator.Models;
using System.Threading;
using System.Threading.Tasks;

namespace OrderSimulator.Handlers
{
    public class OrderCreatedHandler : IPipelineBehavior<Order, Unit>
    {
        private readonly IHubContext<OrderingHub> _hubContext;

        public OrderCreatedHandler(IHubContext<OrderingHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task<Unit> Handle(Order request, CancellationToken cancellationToken, RequestHandlerDelegate<Unit> next)
        {
            await Task.Delay(250).ConfigureAwait(false);
            await _hubContext.Clients.All.SendAsync("order-created", request);
            return await next();
        }
    }
}