using SpaceRobots.Domain.Entities;
using SpaceRobots.Domain.ValueObjects;

namespace SpaceRobots.Application.Database
{
    public interface IRobotRepository
    {
        Task<Robot> AddRobot(string name, GridPosition startingPosition, Orientation startingOrientation, Area startingArea);
        Task<Robot?> GetRobotByName(string name);
        Task RemoveRobot(string name);
        Task SaveRobot(Robot robot);
    }
}