using AutoFixture;
using ConsoleApp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    public class LoggerTest
    {
        [Fact]
        public void Log_AnyMessage_EventInvokedOnce()
        {
            //Arrange
            var log = new Fixture().Create<string>();
            var logger = new Logger();
            var result = 0;
            logger.MessageLogged += (sender, e) => { result++; };

            //Act
            logger.Log(log);

            //Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void Log_AnyMessage_ValidEventInvoked()
        {
            //Arrange
            var log = new Fixture().Create<string>();
            var logger = new Logger();
            object? eventSender = null;
            Logger.LoggerEventArgs? loggerEventArgs = null;
            DateTime timeStop = default;
            logger.MessageLogged += (sender, e) => { timeStop = DateTime.Now; eventSender = sender; loggerEventArgs = e as Logger.LoggerEventArgs; };

            var timeStart = DateTime.Now;
            //Act
            logger.Log(log);

            //Assert
            Assert.Equal(logger, eventSender);
            Assert.NotNull(loggerEventArgs);
            Assert.Equal(log, loggerEventArgs.Message);
            Assert.InRange(loggerEventArgs.DateTime, timeStart, timeStop);
        }

        [Fact]
        public void GetLogAsync_DateRange_LoggerMessage()
        {
            //Arrange
            var log = new Fixture().Create<string>();
            var logger = new Logger();
            DateTime rangeFrom = DateTime.Now;
            logger.Log(log);
            DateTime rangeTo = DateTime.Now;

            //Act
            var result = logger.GetLogsAsync(rangeFrom, rangeTo).Result;

            //Assert
            var splited = result.Split(": ");
            Assert.Equal(2, splited.Length);
            Assert.Equal(log, splited[1]);
            Assert.True(DateTime.TryParseExact(splited[0], "dd.MM.yyyy hh:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
        }
    }
}
