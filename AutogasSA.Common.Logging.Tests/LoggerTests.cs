using FakeItEasy;
using FluentAssertions;
using System.Reflection;
using AutogasSA.Common.Logging.Interfaces;
using System.ComponentModel;

namespace AutogasSA.Common.Logging.Tests
{
    public class LoggerTests
    {
        private readonly ILoggerService _logger;
        public LoggerTests()
        {
            var executableDirectory = Directory.GetCurrentDirectory();
            var solutionDirectory = Directory.GetParent(executableDirectory).Parent.Parent.Parent;
            var mainAppDirectory = Path.Combine(solutionDirectory.FullName, "DockerTester");
            var appSettingsPath = Path.Combine(mainAppDirectory, "appsettings.json");
            var targetPath = Path.Combine(executableDirectory, "appsettings.json");
            File.Copy(appSettingsPath, targetPath, true);
            _logger = new Logger();
        }

        private string InvokePrivateMethod_GetLogFilePathTemplate()
        {
            var methodInfo = typeof(Logger).GetMethod("GetLogFilePathTemplate", BindingFlags.NonPublic | BindingFlags.Instance);

            return (string)methodInfo.Invoke(_logger, null);
        }

        private bool InvokePrivateMethod_ValidSettings()
        {
            var methodInfo = typeof(Logger).GetMethod("ValidSettings", BindingFlags.NonPublic | BindingFlags.Instance);

            return (bool)methodInfo.Invoke(_logger, null);
        }

        [Fact]
        public void Logger_ValidSettings_ShouldReturnTrue()
        {
            // Arrange

            // Act
            var flag = InvokePrivateMethod_ValidSettings();


            // Assert
            flag.Should().BeTrue();

        }


        [Fact]
        public void Logger_GetLogFilePathTemplate_ShouldReturnValidPath()
        {
            // Arrange      

            // Act
            var path = InvokePrivateMethod_GetLogFilePathTemplate();


            // Assert
            path.Should().NotBeNullOrEmpty();
        }
    }
}