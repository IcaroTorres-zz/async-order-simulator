using OrderSimulator.Models;
using System;

namespace OrderSimulator.Extensions
{
    public static class StatusExtensions
    {
        public static Order ApproveOrCanceled(this Order order)
        {
            var even = DateTime.UtcNow.Millisecond % 4 == 0;
            order.Status = even ? OrderStatus.Canceled : OrderStatus.Approved;
            return order;
        }

        public static Order ProcessedOrFailed(this Order order)
        {
            var even = DateTime.UtcNow.Millisecond % 5 == 0;
            order.Status = even ? OrderStatus.Failed : OrderStatus.Processed;
            return order;
        }

        public static string Name(this OrderStatus status)
        {
            return Enum.GetName(typeof(OrderStatus), status).ToLowerInvariant();
        }
    }
}
