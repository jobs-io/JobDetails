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

        public Config(string[] args)
        {
            this.JobDetailsPath = args[0];
            this.Source = args[1];
            this.AppConfigPath = args[2];
        }
    }

    public class DataStore : IDataStore {
        private readonly IDictionary<string, string> JobDetails;
    
        public DataStore(string path)
        {
            this.JobDetails = JsonConvert.DeserializeObject<IDictionary<string, string>>(File.ReadAllText(path));
        }

         public IDictionary<string, string> GetJob(string key) {
             var value = this.JobDetails[key];
             return new Dictionary<string, string>() { { key, value } };
         }
         public void CreateJob(string key, string value) {
             this.JobDetails.Add(key, value);
         }
         public bool JobExists(string key) {
             return this.JobDetails.ContainsKey(key);
         }

    }

    class Program
    {
        static void Main(string[] args)
        {
            var config = new Config(args);

            System.Console.WriteLine(config.JobDetailsPath);
            System.Console.WriteLine(config.Source);            

            using(var reader = new StreamReader(config.JobDetailsPath)) {
                System.Console.WriteLine(reader.ReadToEnd());
            }

            var appConfig = File.ReadAllText(config.AppConfigPath);
            System.Console.WriteLine(appConfig);
            new App(new JobDetailsConfig(appConfig.Replace("{source}", config.Source)), new DataStore(config.JobDetailsPath), new HttpClient());
        }
    }
}
