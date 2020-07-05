using MediatR;
using OrderSimulator.Models;
using System;
using System.Collections.Generic;

namespace OrderSimulator.Commands
{
    public class OrderCommand : IRequest
    {
        public string CustomerName { get; set; }
        public List<Product> Items { get; set; }
        public string Voucher { get; set; }

        public Guid RequestId { get; set; } = Guid.NewGuid();
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
    }
}