using System.Net;

namespace SpaceRobots.Application.Responses
{
    public record RobotResponse
    {
        public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.OK;
        public string ErrorMessage { get; init; } = String.Empty;
    }
}
