using MediatR;
using SpaceRobots.Application.Database;
using SpaceRobots.Application.Responses;
using SpaceRobots.Domain.ValueObjects;
using System.Net;

namespace SpaceRobots.Application.Commands
{
    public class MoveRobot
    {
        public record Command(string Name, string Commands) : IRequest<Response>;

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
                if (robot is null)
                {
                    return new Response()
                    {
                        Name = request.Name,
                        StatusCode = HttpStatusCode.NotFound,
                        ErrorMessage = "Robot Not Found"
                    };
                }
                foreach (char c in request.Commands.ToCharArray())
                {
                    switch (Char.ToUpper(c))
                    {
                        case 'A':
                            robot.Advance();
                            break;
                        case 'L':
                            robot.TurnLeft();
                            break;
                        case 'R':
                            robot.TurnRight();
                            break;
                        default:
                            break;
                    }
                }
                
                await repository.SaveRobot(robot);
                return new Response()
                {
                    Name = robot.Name,
                    Position = robot.Position,
                    Orientation = robot.Orientation,
                    AssignedArea = robot.AssignedArea,
                    IsOutOfBounds = robot.IsOutOfBounds()
                };
            }
        }

        public record Response : RobotResponse
        {
            public string Name { get; init; } = String.Empty;
            public GridPosition? Position { get; init; }
            public Orientation? Orientation { get; init; }
            public Area AssignedArea { get; init; }
            public bool IsOutOfBounds { get; init; }
        }
    }
}
