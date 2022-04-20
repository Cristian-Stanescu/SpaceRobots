using SpaceRobots.Domain.ValueObjects;

namespace SpaceRobots.Dtos
{
    public class RobotDto
    {
        public string Name { get; set; } = String.Empty;
        public GridPosition Position { get; set; }
        public Orientation Orientation { get; set; }
        public Area AssignedArea { get; set; }

    }
}
