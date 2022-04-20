using MediatR;
using SpaceRobots.Application.Database;
using SpaceRobots.Application.Responses;

namespace SpaceRobots.Application.Commands
{
    public static class RemoveRobot
    {
        public record Command(string Name) : IRequest<Response>;

        public class Handler : IRequestHandler<Command, Response>
        {
            private readonly IRobotRepository repository;

            public Handler(IRobotRepository repository)
            {
                this.repository = repository;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                await repository.RemoveRobot(request.Name);
                return new Response();
            }
        }

        public record Response : RobotResponse
        {
        }
    }
}
