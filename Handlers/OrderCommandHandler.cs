using MediatR;
using OrderSimulator.Commands;
using OrderSimulator.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderSimulator.Handlers
{
    public class OrderCommandHandler : AsyncRequestHandler<OrderCommand>
    {
        private readonly IMediator _mediator;

        public OrderCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected override Task Handle(OrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                RequestId = request.RequestId,
                Customer = request.CustomerName,
                Items = request.Items,
                IssuedAt = request.IssuedAt,
            };

            if (!string.IsNullOrWhiteSpace(request.Voucher))
            {
                order.Voucher = DateTime.UtcNow.Millisecond % 2 == 0
                  ? new FixedVoucher(request.Voucher) as Voucher
                  : new PercentVoucher(request.Voucher);
            }

            return _mediator.Send(order);
        }
    }
}