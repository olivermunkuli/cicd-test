using Microsoft.Extensions.Configuration;

namespace AutogasSA.Common.Utilities
{
    public static class Configuration
    {
        public static IConfigurationRoot Setting()
        {
            return new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }

    }
}