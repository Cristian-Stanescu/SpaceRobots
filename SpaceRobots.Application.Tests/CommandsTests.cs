using NSubstitute;
using SpaceRobots.Application.Commands;
using SpaceRobots.Application.Database;
using SpaceRobots.Domain.Entities;
using SpaceRobots.Domain.ValueObjects;
using System.Net;
using System.Threading;
using Xunit;

namespace SpaceRobots.Application.Tests
{
    public class CommandsTests
    {
        private readonly IRobotRepository repository = Substitute.For<IRobotRepository>();

        [Fact]
        public async void CanHandleAddRobotCommand()
        {
            //Arrange
            string robotName = "TestRobot";
            GridPosition robotPosition = new(0, 0);
            Area robotAssignedArea = new(0, 0, 1, 1);
            var robot = new Robot(robotName, robotPosition, Orientation.North, robotAssignedArea);
            repository.AddRobot(robotName, robotPosition, Orientation.North, robotAssignedArea).Returns(robot);
            var sut = new AddRobot.Handler(repository);

            //Act
            var response = await sut.Handle(new AddRobot.Command(robotName, robotPosition, Orientation.North, robotAssignedArea), new CancellationToken());

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(robot.Name, response.Name);
            await repository.Received().AddRobot(robotName, robotPosition, Orientation.North, robotAssignedArea);
        }

        [Fact]
        public async void CanHandleRemoveRobotCommand()
        {
            //Arrange
            string robotName = "TestRobot";
            var sut = new RemoveRobot.Handler(repository);

            //Act
            var response = await sut.Handle(new RemoveRobot.Command(robotName), new CancellationToken());

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            await repository.Received().RemoveRobot(robotName);
        }

        public static TheoryData<string, GridPosition, Orientation, bool> StartingAndExpectedOrientationsForLeftTurn => new()
        {
            { "RAR", new GridPosition(2, 1), Orientation.South, true },
            { "ALRA", new GridPosition(1, -1), Orientation.North, true },
            { "RARAL", new GridPosition(2, 2), Orientation.East, true },
            { "ALALALA", new GridPosition(1, 1), Orientation.East, false },
            { "AALALAAR", new GridPosition(0, 1), Orientation.West, false },
        };

        [Theory]
        [MemberData(nameof(StartingAndExpectedOrientationsForLeftTurn))]
        public async void CanHandleMoveRobotCommand(string commands, GridPosition expectedFinalPosition,
            Orientation expectedFinalOrientation, bool expectedOutOfBounds)
        {
            //Arrange
            string robotName = "TestRobot";
            var robot = new Robot(robotName, new(1, 1), Orientation.North, new(0, 0, 1, 1));
            repository.GetRobotByName(robotName).Returns(robot);
            var sut = new MoveRobot.Handler(repository);

            //Act
            var response = await sut.Handle(new MoveRobot.Command(robotName, commands), new CancellationToken());

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(robot.Name, response.Name);
            Assert.Equal(expectedFinalPosition, robot.Position);
            Assert.Equal(expectedFinalOrientation, robot.Orientation);
            Assert.Equal(expectedOutOfBounds, robot.IsOutOfBounds());
            await repository.Received().GetRobotByName(robotName);
            await repository.Received().SaveRobot(robot);
        }
    }
}
