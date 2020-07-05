using MediatR;
using Microsoft.AspNetCore.SignalR;
using OrderSimulator.Extensions;
using OrderSimulator.Hubs;
using OrderSimulator.Models;
using System.Threading;
using System.Threading.Tasks;

namespace OrderSimulator.Handlers
{
    public class OrderApprovedHandler : IPipelineBehavior<Order, Unit>
    {
        private readonly IHubContext<OrderingHub> _hubContext;

        public OrderApprovedHandler(IHubContext<OrderingHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task<Unit> Handle(Order request, CancellationToken cancellationToken, RequestHandlerDelegate<Unit> next)
        {
            await Task.Delay(250).ConfigureAwait(false);
            request.ApproveOrCanceled();
            await _hubContext.Clients.All.SendAsync($"order-{request.Status.Name()}", request);

            return request.Status == OrderStatus.Approved ? await next() : Unit.Value;
        }
    }
}