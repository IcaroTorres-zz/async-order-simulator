using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderSimulator.Commands;
using System.Threading;

namespace OrderSimulator.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator mediator;
        public OrdersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public IActionResult Post([FromBody] OrderCommand command, CancellationToken cancellationToken = default)
        {
            mediator.Send(command, cancellationToken).ConfigureAwait(false);
            return Accepted(command.RequestId);
        }
    }
}