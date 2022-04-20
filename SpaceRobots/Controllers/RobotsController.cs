using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpaceRobots.Application.Commands;
using SpaceRobots.Application.Queries;
using SpaceRobots.Dtos;
using System.Net;

namespace SpaceRobots.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    [Route("api/v1/robots")]
    public class RobotsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RobotsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(Name = "AddRobot")]
        public async Task<IActionResult> AddRobot(RobotDto robot)
        {
            var response = await _mediator.Send(new AddRobot.Command(robot.Name, robot.Position, robot.Orientation, robot.AssignedArea));
            return response.StatusCode == HttpStatusCode.BadRequest ? BadRequest(response.ErrorMessage) : Ok(response);
        }

        [HttpDelete("{name}", Name = "RemoveRobot")]
        public async Task<IActionResult> RemoveRobot(string name)
        {
            return Ok(await _mediator.Send(new RemoveRobot.Command(name)));
        }

        [HttpGet("{name}", Name = "GetRobotState")]
        public async Task<IActionResult> GetRobotState(string name)
        {
            var response = await _mediator.Send(new GetRobotState.Query(name));
            return response.StatusCode == HttpStatusCode.NotFound ? NotFound() : Ok(response);
        }

        [HttpPost("{name}/process-commands", Name = "ProcessCommands")]
        public async Task<IActionResult> ProcessCommands(string name, [FromQuery] string commands)
        {
            var response = await _mediator.Send(new MoveRobot.Command(name, commands));
            return response.StatusCode == HttpStatusCode.NotFound ? NotFound() : Ok(response);
        }
    }
}
