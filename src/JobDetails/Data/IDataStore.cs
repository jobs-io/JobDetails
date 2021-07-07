using System.Collections.Generic;

namespace JobDetails.Data
{
    public interface IDataStore
    {
         IDictionary<string, string> GetJob(string key);
         void CreateJob(string key, string value);
         bool JobExists(string key);
    }
}