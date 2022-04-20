using SpaceRobots.Domain.Entities;
using SpaceRobots.Domain.ValueObjects;

namespace SpaceRobots.Application.Database
{
    public class RobotRepository : IRobotRepository
    {
        private Dictionary<string, Robot> Robots { get; } = new Dictionary<string, Robot>();

        public async Task<Robot> AddRobot(string name, GridPosition startingPosition, Orientation startingOrientation, Area startingArea)
        {
            var robot = new Robot(name, startingPosition, startingOrientation, startingArea);
            Robots.TryAdd(name, robot);
            return await Task.FromResult(robot);
        }

        public Task RemoveRobot(string name)
        {
            Robots.Remove(name, out Robot _);
            return Task.CompletedTask;
        }

        public async Task<Robot?> GetRobotByName(string name)
        {
            Robots.TryGetValue(name, out Robot? robot);
            return await Task.FromResult(robot);
        }

        public Task SaveRobot(Robot robot)
        {
            return Task.CompletedTask;
        }
    }
}