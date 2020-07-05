using Microsoft.AspNetCore.SignalR;
using OrderSimulator.Commands;
using OrderSimulator.Models;
using System.Threading.Tasks;

namespace OrderSimulator.Hubs
{
    public class OrderingHub : Hub
    {
        public async Task NotifyReceivedOrder(OrderCommand command)
        {
            await Clients.Caller.SendAsync("order-received", command);
        }

        public async Task NotifyCreatedOrder(Order order)
        {

            await Clients.Caller.SendAsync("order-created", order);
        }

        public async Task NotifyProcessedOrder(Order order)
        {
            await Clients.Caller.SendAsync("order-processed", order);
        }
    }
}