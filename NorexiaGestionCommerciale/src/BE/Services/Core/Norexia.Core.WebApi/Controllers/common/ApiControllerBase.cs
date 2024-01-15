using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Norexia.Core.WebApi.Controllers.common;

[Route("api/[controller]")]
[ApiController]
public class ApiControllerBase : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
