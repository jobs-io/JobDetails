// using System;
// using JobDetails;
using JobDetails.Config;
// using JobDetails.Data;
// using JobDetails.Core;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using JobDetails.Console.Data;
using JobDetails.Console.Core;

namespace JobDetails.Console
{

    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new JobDetails.Console.Core.Config(args);
            if (config.Action == "list") {
                var data = JsonConvert.DeserializeObject<IDictionary<string, string>>(File.ReadAllText(config.JobDetailsPath));
                foreach (var key in data.Keys)
                {
                    System.Console.WriteLine(key);
                }
            } else {
                System.Console.WriteLine(config.JobDetailsPath);
                System.Console.WriteLine(config.Source);            

                var appConfig = File.ReadAllText(config.AppConfigPath);
                System.Console.WriteLine(config.Action);
                var app = new App(new JobDetailsConfig(appConfig.Replace("{source}", config.Source)), new DataStore(config.JobDetailsPath), new HttpClient());

                var job = await app.GetJob();
                System.Console.WriteLine($"job: title: {job.Title}");
                System.Console.WriteLine($"job: company: {job.Company}");
            }
        }
    }
}
