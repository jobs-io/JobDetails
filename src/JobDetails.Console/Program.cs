using System;
using JobDetails;
using JobDetails.Config;
using JobDetails.Data;
using JobDetails.Core;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace JobDetails.Console
{

    public class HttpClient : IHttpClient
    {
        public Task<System.Net.Http.HttpResponseMessage> GetAsync(string requestUri)
        {
            return new System.Net.Http.HttpClient().GetAsync(requestUri);
        }
    }

    public class Config {
        public readonly string JobDetailsPath;
        public readonly string Source;
        public readonly string AppConfigPath;
        public readonly string Action;

        public Config(string[] args)
        {
            this.JobDetailsPath = args[0];
            this.Source = args[1];
            this.AppConfigPath = args[2];
            if (args.Length > 3) 
                this.Action = args[3];
        }
    }

    public class DataStore : IDataStore {
        private readonly IDictionary<string, string> JobDetails;
        private readonly string Path;
    
        public DataStore(string path)
        {
            this.Path = path;
            this.JobDetails = JsonConvert.DeserializeObject<IDictionary<string, string>>(File.ReadAllText(path));
        }

         public IDictionary<string, string> GetJob(string key) {
             var value = this.JobDetails[key];
             return new Dictionary<string, string>() { { key, value } };
         }
         public void CreateJob(string key, string value) {
             this.JobDetails.Add(key, value);
             File.WriteAllText(this.Path, JsonConvert.SerializeObject(this.JobDetails));
         }
         public bool JobExists(string key) {
             return this.JobDetails.ContainsKey(key);
         }

    }

    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new Config(args);
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
