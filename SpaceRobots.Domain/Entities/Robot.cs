using SpaceRobots.Domain.ValueObjects;

namespace SpaceRobots.Domain.Entities
{
    public class Robot
    {
        public Robot(string name, GridPosition position, Orientation orientation, Area assignedArea)
        {
            Id = Guid.NewGuid();
            Name = name;
            Position = position;
            Orientation = orientation;
            AssignedArea = assignedArea;
        }

        public Guid Id { get; }
        public string Name { get; init; }
        public GridPosition Position { get; private set; }
        public Orientation Orientation { get; private set; }
        public Area AssignedArea { get; init; }

        public void Advance()
        {
            switch (Orientation)
            {
                case Orientation.North:
                    Position = Position with { Y = Position.Y - 1 };
                    break;
                case Orientation.East:
                    Position = Position with { X = Position.X + 1 };
                    break;
                case Orientation.South:
                    Position = Position with { Y = Position.Y + 1 };
                    break;
                case Orientation.West:
                    Position = Position with { X = Position.X - 1 };
                    break;
                default:
                    break;
            }
        }

        public void TurnLeft()
        {
            int orientationIndex = (int)Orientation;
            int leftOrientationIndex = (orientationIndex + 3) % 4;
            Orientation = (Orientation)leftOrientationIndex;
        }

        public void TurnRight()
        {
            int orientationIndex = (int)Orientation;
            int rightOrientationIndex = (orientationIndex + 1) % 4;
            Orientation = (Orientation)rightOrientationIndex;
        }

        public bool IsOutOfBounds()
        {
            if (Position.X < AssignedArea.TopLeft.X || Position.X > AssignedArea.BottomRight.X)
                return true;
            if (Position.Y < AssignedArea.TopLeft.Y || Position.Y > AssignedArea.BottomRight.Y)
                return true;
            return false;
        }
    }
}