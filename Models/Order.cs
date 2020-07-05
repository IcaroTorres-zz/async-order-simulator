using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderSimulator.Models
{
    public class Order : IRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid RequestId { get; set; }
        public string Customer { get; set; }
        public List<Product> Items { get; set; } = new List<Product>();
        public Voucher Voucher { get; set; } = new NoVoucher();
        public decimal OrderAmount => Items.Sum(i => i.Amount);
        public decimal Discount => Voucher.Calculate(OrderAmount);
        public decimal TotalAmount => OrderAmount - Discount;
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
        public DateTime ProcessedAt { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
    }

    public enum OrderStatus { Pending = 1, Canceled, Approved, Processed, Failed }
}