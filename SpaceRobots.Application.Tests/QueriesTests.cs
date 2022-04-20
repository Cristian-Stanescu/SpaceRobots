using NSubstitute;
using SpaceRobots.Application.Database;
using SpaceRobots.Application.Queries;
using SpaceRobots.Domain.Entities;
using SpaceRobots.Domain.ValueObjects;
using System.Net;
using System.Threading;
using Xunit;

namespace SpaceRobots.Application.Tests
{
    public class QueriesTests
    {
        private readonly IRobotRepository repository = Substitute.For<IRobotRepository>();

        [Fact]
        public async void CanHandleRemoveRobotCommand()
        {
            //Arrange
            string robotName = "TestRobot";
            var robot = new Robot(robotName, new(1, 1), Orientation.North, new(0, 0, 1, 1));
            repository.GetRobotByName(robotName).Returns(robot);
            var sut = new GetRobotState.Handler(repository);

            //Act
            var response = await sut.Handle(new GetRobotState.Query(robotName), new CancellationToken());

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(robot.Name, response.Name);
            Assert.Equal(robot.Position, response.Position);
            Assert.Equal(robot.Orientation, response.Orientation);
            Assert.Equal(robot.IsOutOfBounds(), response.IsOutOfBounds);
            await repository.Received().GetRobotByName(robotName);
        }

    }
}
