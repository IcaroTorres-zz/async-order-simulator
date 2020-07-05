using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using OrderSimulator.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrderSimulator.Handlers
{
    public class RequestValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly IHubContext<OrderingHub> _hubContext;

        public RequestValidatorBehavior(IEnumerable<IValidator<TRequest>> validators, IHubContext<OrderingHub> hubContext)
        {
            _validators = validators;
            _hubContext = hubContext;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext(request);
            var failures = _validators
                .Select(async x => await x.ValidateAsync(context, cancellationToken))
                .SelectMany(x => x.Result.Errors)
                .Where(x => x != null)
                .ToList();

            if (failures.Any())
            {
                await _hubContext.Clients.All.SendAsync("order-error", failures);
                return (TResponse)Activator.CreateInstance(typeof(TResponse));
            }
            else return await next();
        }
    }
}