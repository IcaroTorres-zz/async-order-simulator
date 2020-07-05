using FluentValidation;
using OrderSimulator.Commands;

namespace OrderSimulator.Validators
{
    public class OrderCommandValidator : AbstractValidator<OrderCommand>
    {
        public OrderCommandValidator()
        {
            RuleFor(x => x.CustomerName).NotEmpty();
            RuleFor(x => x.Items).NotEmpty();
        }
    }
}