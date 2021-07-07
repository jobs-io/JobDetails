using System.Net.Http;
using System.Threading.Tasks;
using JobDetails.Config;
using JobDetails.Core;
using JobDetails.Data;

namespace JobDetails
{
    public class App
    {
        private readonly JobDetailsConfig config;
        private readonly IDataStore store;
        private readonly IHttpClient httpClient;
        public App(JobDetailsConfig config, IDataStore store, IHttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.config = config;
            this.store = store;
        }

        public async ValueTask<Job> GetJob()
        {
            store.JobExists(config.Source);
            var results = await httpClient.GetAsync(config.Source);

            var html = await results.Content.ReadAsStringAsync();
            var htmlReader = new HtmlReader.Reader(html);
            var scriptTemplate = config.Job.ScriptTemplate;
            var script = htmlReader.ParseScript(config.Job.Path, scriptTemplate);

            var jsonReader = new JsonReader.Reader(script);
            return new Job(jsonReader, this.config);
        }
    }
}