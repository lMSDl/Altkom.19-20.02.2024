using ConsoleApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    [TestClass]
    public class GardenTest
    {
        [TestMethod]
        public void Plant_ValidName_True()
        {
            //Arrange
            const int MINIMAL_VALID_GARDEN_SIZE = 1;
            const string VALID_NAME = "a";
            Garden garden = new(MINIMAL_VALID_GARDEN_SIZE);

            //Act
            bool result = garden.Plant(VALID_NAME); 

            //Assert
            //sprawdzamy tylko jedną rzecz
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Plant_OverflowGarden_False()
        {
            //Arrange
            const int MINIMAL_VALID_GARDEN_SIZE = 1;
            const string VALID_NAME_1 = "a";
            const string VALID_NAME_2 = "b";
            Garden garden = new(MINIMAL_VALID_GARDEN_SIZE);
            garden.Plant(VALID_NAME_1);

            //Act
            bool result = garden.Plant(VALID_NAME_2);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Plant_NullName_ArgumentNullException()
        {
            //Arrange
            const int INSIGNIFICANT_SIZE = default;
            Garden garden = new(INSIGNIFICANT_SIZE);
            const string? NULL_NAME = null;

            //Act
            garden.Plant(NULL_NAME);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\n")]
        [DataRow("\t")]
        public void Plant_EmptyOrWhitespaceName_ArgumentException(string name)
        {
            //Arrange
            const int INSIGNIFICANT_SIZE = default;
            Garden garden = new(INSIGNIFICANT_SIZE);
            const string EXPECTED_PARAM_NAME = "name";
            const string EXPECTED_MESSAGE = "Roślina musi posiadać nazwę!";

            //Act
            Action action = () => garden.Plant(name);

            //Assert
            var exception = Assert.ThrowsException<ArgumentException>(action);
            Assert.AreEqual(EXPECTED_PARAM_NAME, exception.ParamName);
            Assert.IsTrue(exception.Message.Contains(EXPECTED_MESSAGE));
        }

        [TestMethod]
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
            Assert.IsTrue(garden.GetPlants().Contains(EXPECTED_NAME));
        }

        [TestMethod]
        public void GetPlants_CopyOfPlantsCollection()
        {
            //Arrange
            const int INSIGNIFICANT_SIZE = default;
            Garden garden = new(INSIGNIFICANT_SIZE);

            //Act
            var result1 = garden.GetPlants();
            var result2 = garden.GetPlants();

            //Assert
            Assert.AreNotSame(result1, result2);
        }
    }
}
