using System;
using JobDetails;
using System.IO;

namespace JobDetails.Console
{
    public class Config {
        public readonly string JobDetailsPath;
        public readonly string Source;

        public Config(string[] args)
        {
            this.JobDetailsPath = args[0];
            this.Source = args[1];
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var config = new Config(args);

            System.Console.WriteLine(config.JobDetailsPath);
            System.Console.WriteLine(config.Source);
        }
    }
}
