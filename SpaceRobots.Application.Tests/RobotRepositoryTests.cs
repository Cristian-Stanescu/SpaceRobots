using SpaceRobots.Application.Database;
using SpaceRobots.Domain.ValueObjects;
using System.Threading.Tasks;
using Xunit;

namespace SpaceRobots.Application.Tests
{
    public class RobotRepositoryTests
    {
        [Fact]
        public async Task CanGetRobotByName()
        {
            //Arrange
            var robotName = "TestRobot";
            var sut = new RobotRepository();
            await sut.AddRobot(robotName, new GridPosition(1, 1), Orientation.North, new Area(0, 0, 1, 1));

            //Act
            var robotFromRepository = await sut.GetRobotByName(robotName);

            //Assert
            Assert.NotNull(robotFromRepository);
            Assert.Equal(robotName, robotFromRepository!.Name);
        }

        [Fact]
        public async Task CantGetRobotByNameIfNotCreated()
        {
            //Arrange
            var robotName = "TestRobot";
            var sut = new RobotRepository();

            //Act
            var robotFromRepository = await sut.GetRobotByName(robotName);

            //Assert
            Assert.Null(robotFromRepository);
        }

        [Fact]
        public async void CantGetRobotByNameIfDeleted()
        {
            //Arrange
            var robotName = "TestRobot";
            var sut = new RobotRepository();
            await sut.AddRobot(robotName, new GridPosition(1, 1), Orientation.North, new Area(0, 0, 1, 1));
            await sut.RemoveRobot(robotName);

            //Act
            var robotFromRepository = await sut.GetRobotByName(robotName);

            //Assert
            Assert.Null(robotFromRepository);
        }
    }
}
