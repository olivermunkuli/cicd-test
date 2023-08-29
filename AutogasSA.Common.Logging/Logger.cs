using Serilog;
using Serilog.Events;
using AutogasSA.Common.Utilities;
using AutogasSA.Common.Logging.Interfaces;

namespace AutogasSA.Common.Logging
{
    public class Logger : ILoggerService
    {
        private ILogger _logger;
        public Logger()
        {
            _logger = new LoggerConfiguration()
                 .MinimumLevel.Debug()
                 .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                 .Enrich.FromLogContext()
                 .WriteTo.File(GetLogFilePathTemplate(),
                             restrictedToMinimumLevel: LogEventLevel.Information,
                             outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {Message:lj}{NewLine}{Exception}",
                             fileSizeLimitBytes: 5242880,
                             rollingInterval: RollingInterval.Hour,
                             rollOnFileSizeLimit: true,
                             retainedFileCountLimit: 13
                            )   
                 .CreateLogger();
        }


        public void LogInformation(string message)
        {
            _logger.Information(message);
        }

        public void LogWarning(string message)
        {
            _logger.Warning(message);
        }

        public void LogError(string message, Exception? ex)
        {
            _logger.Error(ex, message);
        }

        public void LogDebug(string message)
        {
            _logger.Debug(message);
        }

        private string GetLogFilePathTemplate()
        {
            var logTo = Configuration.Setting()["Logs:LogFolder"]?.ToLower() ?? "programdata";
            var folderName = Configuration.Setting()["Logs:FolderName"]?.ToLower() ?? "";
            var appName = Configuration.Setting()["App:Name"]?.ToLower() ?? "";
            var os = Configuration.Setting()["OS:Platform"]?.ToLower() ?? "";

            if (logTo == "custom" && os == "linux")
            {
                var customLogPath = $"{folderName}";
                return Path.Combine(customLogPath, "log-.txt");
            }

            if (logTo == "custom" && os == "windows")
            {
                var customLogPath = $"{folderName}";
                return Path.Combine(customLogPath, "log-.txt");
            }

            if (logTo == "programdata" && os == "windows")
            {
                var programDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                var logPath = $"{programDataFolderPath}\\{appName}\\logs";

                return Path.Combine(logPath, "log-.txt");
            }

            return "";

        }

        private bool ValidSettings()
        {
            var endPoint = Configuration.Setting()["APIGateway:Endpoint"];
            if (endPoint == null) return false;

            var contentType = Configuration.Setting()["APIGateway:ContentType"];
            if (contentType == null) return false;

            var input = Configuration.Setting()["Data:Input"];
            if (input == null) return false;

            var archive = Configuration.Setting()["Data:Archive"];
            if (archive == null) return false;

            var logFolder = Configuration.Setting()["Logs:LogFolder"];
            if (logFolder == null) return false;

            var folderName = Configuration.Setting()["Logs:FolderName"];
            if (folderName == null) return false;

            var name = Configuration.Setting()["App:Name"];
            if (name == null) return false;

            var platform = Configuration.Setting()["OS:Platform"];
            if (platform == null) return false;

            return true

        }

    }


}