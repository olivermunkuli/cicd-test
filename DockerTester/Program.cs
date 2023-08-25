using AutogasSA.Common.Utilities;
using AutogasSA.Common.Logging;

namespace DockerTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new Logger();

            var directoryPath = Configuration.Setting()["Data:Input"] ?? "";

            while (true)
            {
                if (Directory.Exists(directoryPath))
                {
                    Console.WriteLine($"Contents of directory: {directoryPath}");
                    logger.LogInformation($"Contents of directory: {directoryPath}");

                    string[] files = Directory.GetFiles(directoryPath);

                    if (files.Length <= 0 ) 
                    {
                        Console.WriteLine("No files found!");
                        logger.LogInformation("No files found!");
                    }
                    else
                    {
                        foreach (string file in files)
                        {
                            Console.WriteLine(Path.GetFileName(file));
                            logger.LogInformation(Path.GetFileName(file));
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Directory does not exist: {directoryPath}");
                    Path.GetFileName($"Directory does not exist: {directoryPath}");
                }
                var pause = 20000;
                Console.WriteLine($"Waiting for {pause} seconds.");
                logger.LogDebug($"Waiting for {pause} seconds.");
                Thread.Sleep(pause);

            }
        }
    }
}
