using ConsoleApp;
using System;
using System.Collections.Generic;
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
            const int MININAL_VALID_GARDEN_SIZE = 1;
            const string VALID_NAME = "a"; //piszemy testy z minimalnym przekazem (paramety o jak namjniejszym polem do interpetacji) i opisujemy swoje intencje
            Garden garden = new(MININAL_VALID_GARDEN_SIZE);

            //Act
            bool result = garden.Plant(VALID_NAME); 

            //Assert
            //sprawdzamy tylko jedną rzecz
            Assert.True(result);
        }

        [Fact]
        public void Plant_OverflowGarden_False()
        {
            //Arrange
            const int MININAL_VALID_GARDEN_SIZE = 1;
            const string VALID_NAME_1 = "a";
            const string VALID_NAME_2 = "b";
            Garden garden = new(MININAL_VALID_GARDEN_SIZE);
            garden.Plant(VALID_NAME_1);

            //Act
            bool result = garden.Plant(VALID_NAME_2);

            //Assert
            Assert.False(result);
        }

    }
}
