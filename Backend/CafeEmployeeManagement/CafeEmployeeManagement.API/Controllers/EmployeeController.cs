using CafeEmployeeManagement.Application.Features.Employees.Commands.CreateEmployee;
using CafeEmployeeManagement.Application.Features.Employees.Commands.DeleteEmployee;
using CafeEmployeeManagement.Application.Features.Employees.Commands.UpdateEmployee;
using CafeEmployeeManagement.Application.Features.Employees.Queries.GetEmployees;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CafeEmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator mediator;

        public EmployeeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetEmployees([FromQuery] GetEmployeesQuery query)
        {
            var response = await mediator.Send(query);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(string id)
        {
            var query = new GetEmployeeByIdQuery();
            query.EmployeeId = id;

            var response = await mediator.Send(query);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> CreateEmployee([FromBody] CreateEmployeeCommand command)
        {
            var response = await mediator.Send(command);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(string id, [FromBody] UpdateEmployeeCommand command)
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
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var command = new DeleteEmployeeCommand
            {
                EmployeeId = id
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
