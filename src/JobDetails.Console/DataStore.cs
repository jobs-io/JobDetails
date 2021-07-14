using System.IO;
using System.Collections.Generic;
using JobDetails.Data;
using Newtonsoft.Json;

namespace JobDetails.Console
{
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
}
