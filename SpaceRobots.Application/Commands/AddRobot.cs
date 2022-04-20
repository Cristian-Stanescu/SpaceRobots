using MediatR;
using SpaceRobots.Application.Database;
using SpaceRobots.Application.Responses;
using SpaceRobots.Domain.ValueObjects;
using System.Net;

namespace SpaceRobots.Application.Commands
{
    public static class AddRobot
    {
        public record Command(string Name, GridPosition StartingPosition, Orientation StartingOrientation, Area StartingArea) : IRequest<Response>;

        public class Handler : IRequestHandler<Command, Response>
        {
            private readonly IRobotRepository repository;

            public Handler(IRobotRepository repository)
            {
                this.repository = repository;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var robot = await repository.GetRobotByName(request.Name);
                if (robot is not null)
                {
                    return new Response()
                    {
                        Name = request.Name,
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorMessage = "Robot already exists!"
                    };
                }
                robot = await repository.AddRobot(request.Name, request.StartingPosition, request.StartingOrientation, request.StartingArea);
                return new Response { Name = robot.Name };
            }
        }

        public record Response : RobotResponse
        {
            public string Name { get; init; } = String.Empty;
        }
    }
}
