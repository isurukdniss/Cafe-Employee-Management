using CafeEmployeeManagement.Application.Features.Cafes.Commands.CreateCafe;
using CafeEmployeeManagement.Application.Features.Cafes.Commands.DeleteCafe;
using CafeEmployeeManagement.Application.Features.Cafes.Commands.UpdateCafe;
using CafeEmployeeManagement.Application.Features.Cafes.Queries.GetCafes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CafeEmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CafeController : ControllerBase
    {
        private readonly IMediator mediator;

        public CafeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetCafes([FromQuery] GetCafesQuery query)
        {
            var response = await mediator.Send(query);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCafeById(Guid id)
        {
            var query = new GetCafeByIdQuery();
            query.CafeId = id;

            var response = await mediator.Send(query);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCafe([FromForm] CreateCafeCommand command)
        {
            var response = await mediator.Send(command);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCafe(Guid id, [FromForm] UpdateCafeCommand command)
        {
            command.Id = id;
            var response = await mediator.Send(command);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCafe(Guid id)
        {
            var command = new DeleteCafeCommand
            {
                CafeId = id
            };

            var response = await mediator.Send(command);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
