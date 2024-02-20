using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    public class AbstractTest
    {


        [Fact]
        public void MyTest()
        {
            var myClass = new Mock<MyClass>();

            var result = myClass.Object.DoSth();

            result.Should().Be("");
        }

        public abstract class MyClass
        {

            public string DoSth()
            {
                return "";
            }

        }
    }
}
