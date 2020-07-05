using System;

namespace OrderSimulator.Models
{
    public abstract class Voucher
    {
        public string Code { get; protected set; }
        public decimal Value { get; protected set; }
        public abstract decimal Calculate(decimal amount);
    }

    public class PercentVoucher : Voucher
    {
        public PercentVoucher(string code)
        {
            Code = string.IsNullOrWhiteSpace(code) ? $"PERCENT_PROMO-{Guid.NewGuid()}" : code;
            Value = DateTime.UtcNow.Millisecond % 16;
        }
        public override decimal Calculate(decimal amount)
        {
            var randomPercentage = Value / 100;
            return amount * randomPercentage;
        }
    }

    public class FixedVoucher : Voucher
    {
        public FixedVoucher(string code)
        {
            Code = string.IsNullOrWhiteSpace(code) ? $"FIXED_PROMO-{Guid.NewGuid()}" : code;
            Value = 50;
        }
        public override decimal Calculate(decimal amount)
        {
            return Value;
        }
    }

    public class NoVoucher : Voucher
    {
        public override decimal Calculate(decimal amount)
        {
            return 0;
        }
    }
}