using AutoFixture;
using ConsoleApp;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    public class GardenTest
    {
        [Fact]
        //public void Plant_GivesTrueWhenProvidedValidPlantName
        //public void Plant_PassValidName_ResturnsTrue()
        //<nazwa metody>_<scemariusz>_<oczekiwany rezultat>
        public void Plant_ValidName_True()
        {
            //Arrange
            const int MINIMAL_VALID_GARDEN_SIZE = 1;

            //const string VALID_NAME = "a"; //piszemy testy z minimalnym przekazem (paramety o jak namjniejszym polem do interpetacji) i opisujemy swoje intencje
            string validName = new Fixture().Create<string>();

            Garden garden = new(MINIMAL_VALID_GARDEN_SIZE);

            //Act
            bool result = garden.Plant(validName);

            //Assert
            //sprawdzamy tylko jedną rzecz
            Assert.True(result);
        }

        [Fact]
        public void Plant_OverflowGarden_False()
        {
            //Arrange
            const int MINIMAL_VALID_GARDEN_SIZE = 1;
            var fixture = new Fixture();
            string name1 = fixture.Create<string>();
            string name2 = fixture.Create<string>();
            Garden garden = new(MINIMAL_VALID_GARDEN_SIZE);
            garden.Plant(name1);

            //Act
            bool result = garden.Plant(name2);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void Plant_NullName_ArgumentNullException()
        {
            //Arrange
            const int INSIGNIFICANT_SIZE = default;
            Garden garden = new(INSIGNIFICANT_SIZE);
            const string? NULL_NAME = null;
            const string EXPECTED_PARAM_NAME = "name";

            //Act
            Action action = () => garden.Plant(NULL_NAME);

            //Assert
            //var argumentNullException = Assert.ThrowsAny<ArgumentException>(action); //ThrowsAny - uwzględnia dziedziczenie klas wyjątków 
            var argumentNullException = Assert.Throws<ArgumentNullException>(action);
            Assert.Equal(EXPECTED_PARAM_NAME, argumentNullException.ParamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData("\t")]
        public void Plant_EmptyOrWhitespaceName_ArgumentException(string name)
        {
            //Arrange
            const int INSIGNIFICANT_SIZE = default;
            Garden garden = new(INSIGNIFICANT_SIZE);
            const string EXPECTED_PARAM_NAME = "name";
            const string EXPECTED_MESSAGE = "Roślina musi posiadać nazwę!";

            //Act
            var exception = Record.Exception(() => garden.Plant(name));

            //Assert
            Assert.NotNull(exception);
            var argumentExeption = Assert.IsType<ArgumentException>(exception);
            Assert.Equal(EXPECTED_PARAM_NAME, argumentExeption.ParamName);
            Assert.Contains(EXPECTED_MESSAGE, argumentExeption.Message);
        }


        [Fact(Skip = "Replaced by Plant_EmptyOrWhitespaceName_ArgumentException")]
        public void Plant_EmptyName_ArgumentException()
        {
            //Arrange
            const int INSIGNIFICANT_SIZE = default;
            Garden garden = new(INSIGNIFICANT_SIZE);
            const string EMPTY_NAME = "";
            const string EXPECTED_PARAM_NAME = "name";
            const string EXPECTED_MESSAGE = "Roślina musi posiadać nazwę!";

            //Act
            var exception = Record.Exception(() => garden.Plant(EMPTY_NAME));

            //Assert
            Assert.NotNull(exception);
            var argumentExeption = Assert.IsType<ArgumentException>(exception);
            Assert.Equal(EXPECTED_PARAM_NAME, argumentExeption.ParamName);
            Assert.Contains(EXPECTED_MESSAGE, argumentExeption.Message);
        }

        [Fact(Skip = "Replaced by Plant_EmptyOrWhitespaceName_ArgumentException")]
        public void Plant_WhitespaceName_ArgumentException()
        {
            //Arrange
            const int INSIGNIFICANT_SIZE = default;
            Garden garden = new(INSIGNIFICANT_SIZE);
            const string EMPTY_NAME = "  ";
            const string EXPECTED_PARAM_NAME = "name";
            const string EXPECTED_MESSAGE = "Roślina musi posiadać nazwę!";

            //Act
            var exception = Record.Exception(() => garden.Plant(EMPTY_NAME));

            //Assert
            Assert.NotNull(exception);
            var argumentExeption = Assert.IsType<ArgumentException>(exception);
            Assert.Equal(EXPECTED_PARAM_NAME, argumentExeption.ParamName);
            Assert.Contains(EXPECTED_MESSAGE, argumentExeption.Message);
        }

        [Fact]
        public void Plant_ExistingName_ChangedName()
        {
            //Arrange
            const int MINIMAL_VALID_GARDEN_SIZE = 2;
            const string VALID_NAME = "a";
            const string EXPECTED_NAME = VALID_NAME + "2";
            Garden garden = new(MINIMAL_VALID_GARDEN_SIZE);
            garden.Plant(VALID_NAME);

            //Act
            garden.Plant(VALID_NAME);

            //Assert
            Assert.Contains(EXPECTED_NAME, garden.GetPlants());
        }

        [Fact]
        public void GetPlants_CopyOfPlantsCollection()
        {
            //Arrange
            const int INSIGNIFICANT_SIZE = default;
            Garden garden = new(INSIGNIFICANT_SIZE, new Mock<ILogger>().Object);

            //Act
            var result1 = garden.GetPlants();
            var result2 = garden.GetPlants();

            //Assert
            Assert.NotSame(result1, result2); //sprawdzenie instancji
        }

        [Fact]
        public void Plant_ValidName_MessageLogged()
        {
            //Arrange
            var fixture = new Fixture();
            string name = fixture.Create<string>();
            string expectedMessage = $"Roślina {name} została dodana do ogrodu";

            const int MINIMAL_VALID_GARDEN_SIZE = 1;
            var logger = new Mock<ILogger>();
            logger.Setup(x => x.Log(It.Is<string>(expectedMessage, StringComparer.InvariantCulture))).Verifiable();
            //logger.Setup(x => x.Log(It.IsAny<string>())).Verifiable();

            Garden garden = new(MINIMAL_VALID_GARDEN_SIZE, logger.Object);

            //Act
            garden.Plant(name);

            //Assert
            logger.Verify();
        }

        [Fact]
        public void Plant_DuplicatedName_MessageLogged()
        {
            //Arrange
            var fixture = new Fixture();
            string name = fixture.Create<string>();

            const int MINIMAL_VALID_GARDEN_SIZE = 2;
            var logger = new Mock<ILogger>();

            Garden garden = new(MINIMAL_VALID_GARDEN_SIZE, logger.Object);
            garden.Plant(name);

            //Act
            garden.Plant(name);

            //Assert
            logger.Verify(x => x.Log(It.Is<string>(xx => xx.Contains(name + 2))));
            logger.Verify(x => x.Log(It.Is<string>(xx => xx.Contains(name))), Times.Exactly(3));
        }

        [Fact]
        public void GetLastLog_LastLog()
        {
            //Arrange
            var fixture = new Fixture();
            string log1 = fixture.Create<string>();
            string log2 = fixture.Create<string>();

            const int INSIGNIFICANT_SIZE = 0;
            var logger = new Mock<ILogger>();
            logger.Setup(x => x.GetLogsAsync(It.Is<DateTime>(xx => xx == default), It.IsAny<DateTime>()))
                  .ReturnsAsync($"{log1}\n{log2}");

            Garden garden = new(INSIGNIFICANT_SIZE, logger.Object);

            //Act
            var result = garden.GetLastLog();

            //Assert
            result.Should().Be(log2);
        }

    }
}
