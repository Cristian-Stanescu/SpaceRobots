using SpaceRobots.Domain.Entities;
using SpaceRobots.Domain.ValueObjects;
using Xunit;

namespace SpaceRobots.Domain.Tests
{
    public class RobotTests
    {
        public static TheoryData<Orientation, Orientation> StartingAndExpectedOrientationsForLeftTurn => new()
        {
            { Orientation.North, Orientation.West },
            { Orientation.West, Orientation.South },
            { Orientation.South, Orientation.East },
            { Orientation.East, Orientation.North },
        };

        [Theory]
        [MemberData(nameof(StartingAndExpectedOrientationsForLeftTurn))]
        public void RobotCanTurnLeft(Orientation startingOrientation, Orientation expectedOrientation)
        {
            //Arrange
            var robot = new Robot("test", new GridPosition(0, 0), startingOrientation, new Area(0, 0, 1, 1));

            //Act
            robot.TurnLeft();

            //Assert
            Assert.Equal(expectedOrientation, robot.Orientation);
        }

        public static TheoryData<Orientation, Orientation> StartingAndExpectedOrientationsForRightTurn => new()
        {
            { Orientation.North, Orientation.East },
            { Orientation.East, Orientation.South },
            { Orientation.South, Orientation.West },
            { Orientation.West, Orientation.North },
        };

        [Theory]
        [MemberData(nameof(StartingAndExpectedOrientationsForRightTurn))]
        public void RobotCanTurnRight(Orientation startingOrientation, Orientation expectedOrientation)
        {
            //Arrange
            var robot = new Robot("test", new GridPosition(0, 0), startingOrientation, new Area(0, 0, 1, 1));

            //Act
            robot.TurnRight();

            //Assert
            Assert.Equal(expectedOrientation, robot.Orientation);
        }

        public static TheoryData<Orientation, GridPosition> StartingOrientationAndExpectedPositionForAdvancing => new()
        {
            { Orientation.North, new GridPosition(1, 0) },
            { Orientation.East, new GridPosition(2, 1) },
            { Orientation.South, new GridPosition(1, 2) },
            { Orientation.West, new GridPosition(0, 1) },
        };

        [Theory]
        [MemberData(nameof(StartingOrientationAndExpectedPositionForAdvancing))]
        public void RobotCanAdvance(Orientation startingOrientation, GridPosition expectedResult)
        {
            //Arrange
            var robot = new Robot("test", new GridPosition(1, 1), startingOrientation, new Area(0, 0, 1, 1));

            //Act
            robot.Advance();

            //Assert
            Assert.Equal(expectedResult, robot.Position);
        }

        public static TheoryData<GridPosition, Area, bool> StartingPositionAndExpectedResultForGivenAssignedArea => new()
        {
            { new GridPosition(1, 0), Area.EmptyArea, true },
            { new GridPosition(9, 9), new Area(2, 2, 3, 3), true },
            { new GridPosition(3, 6), new Area(2, 2, 3, 3), true },
            { new GridPosition(1, 0), new Area(2, 2, 3, 3), true },
            { new GridPosition(1, 0), new Area(1, 0, 3, 3), false },
            { new GridPosition(1, 0), new Area(0, 0, 3, 3), false },
        };

        [Theory]
        [MemberData(nameof(StartingPositionAndExpectedResultForGivenAssignedArea))]
        public void RobotCanCheckIfOutOfBounds(GridPosition startingPosition, Area assignedArea, bool expectedResult)
        {
            //Arrange
            var robot = new Robot("test", startingPosition, Orientation.North, assignedArea);

            //Act
            var outOfBounds = robot.IsOutOfBounds();

            //Assert
            Assert.Equal(expectedResult, outOfBounds);
        }
    }
}
