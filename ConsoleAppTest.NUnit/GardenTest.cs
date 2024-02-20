using ConsoleApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTest
{

    //[Ignore("")]
    public class GardenTest
    {
        [Test]
        public void Plant_ValidName_True()
        {
            //Arrange
            const int MINIMAL_VALID_GARDEN_SIZE = 1;
            const string VALID_NAME = "a";
            Garden garden = new(MINIMAL_VALID_GARDEN_SIZE);

            //Act
            bool result = garden.Plant(VALID_NAME); 

            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
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
            Assert.That(result, Is.False);
        }

        [Test]
        public void Plant_NullName_ArgumentNullException()
        {
            //Arrange
            const int INSIGNIFICANT_SIZE = default;
            Garden garden = new(INSIGNIFICANT_SIZE);
            const string? NULL_NAME = null;
            const string EXPECTED_PARAM_NAME = "name";

            //Act
            TestDelegate action = () => garden.Plant(NULL_NAME);

            //Assert
            var argumentNullException = Assert.Throws<ArgumentNullException>(action);
            Assert.That(EXPECTED_PARAM_NAME, Is.EqualTo(argumentNullException.ParamName));
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("\n")]
        [TestCase("\t")]
        public void Plant_EmptyOrWhitespaceName_ArgumentException(string name)
        {
            //Arrange
            const int INSIGNIFICANT_SIZE = default;
            Garden garden = new(INSIGNIFICANT_SIZE);
            const string EXPECTED_PARAM_NAME = "name";
            const string EXPECTED_MESSAGE = "Roślina musi posiadać nazwę!";

            //Act
            TestDelegate action = () => garden.Plant(name);

            //Assert
            Assert.Throws(Is.InstanceOf<ArgumentException>().And
                .Property(nameof(ArgumentException.ParamName)).EqualTo(EXPECTED_PARAM_NAME).And
                .Message.Contains(EXPECTED_MESSAGE), action);
        }

        [Test]
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
            Assert.That(garden.GetPlants(), Does.Contain(EXPECTED_NAME));
        }

        [Test]
        public void GetPlants_CopyOfPlantsCollection()
        {
            //Arrange
            const int INSIGNIFICANT_SIZE = default;
            Garden garden = new(INSIGNIFICANT_SIZE);

            //Act
            var result1 = garden.GetPlants();
            var result2 = garden.GetPlants();

            //Assert
            Assert.That(result1, Is.Not.SameAs(result2));
        }
    }
}
