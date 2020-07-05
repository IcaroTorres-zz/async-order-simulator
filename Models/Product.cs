using System;

namespace OrderSimulator.Models
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }
}