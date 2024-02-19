using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    public class ProgramTest
    {

        [Fact]
        public void Main_ConsoleOutput_HelloWorld()
        {
            //Arrange
            var main = typeof(Program).Assembly.EntryPoint;
            using var stringWriter = new StringWriter();
            var consoleWriter = Console.Out;
            Console.SetOut(stringWriter);

            //Act
            main.Invoke(null, new object[] { Array.Empty<string>() });

            //Assert
            Console.SetOut(consoleWriter);
            Assert.Equal("Hello, World!\n", stringWriter.ToString(), ignoreLineEndingDifferences: true);

        }
    }
}
