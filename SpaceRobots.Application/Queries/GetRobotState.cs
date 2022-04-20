using MediatR;
using SpaceRobots.Application.Database;
using SpaceRobots.Application.Responses;
using SpaceRobots.Domain.ValueObjects;
using System.Net;

namespace SpaceRobots.Application.Queries
{
    public class GetRobotState
    {
        public record Query(string Name) : IRequest<Response>;

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IRobotRepository repository;

            public Handler(IRobotRepository repository)
            {
                this.repository = repository;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
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
