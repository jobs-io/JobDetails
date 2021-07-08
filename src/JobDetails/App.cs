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

        private async ValueTask<string> GetHtml() {
            if(store.JobExists(config.Source)) {
                return store.GetJob(config.Source)[config.Source];
            } else {
                var results = await httpClient.GetAsync(config.Source);
                var html = await results.Content.ReadAsStringAsync();
                store.CreateJob(config.Source, html);                
                return html;
            }
        }

        public async ValueTask<Job> GetJob()
        {
            var html = await GetHtml();
            var htmlReader = new HtmlReader.Reader(html);
            var scriptTemplate = config.Job.ScriptTemplate;
            var script = htmlReader.ParseScript(config.Job.Path, scriptTemplate);

            var jsonReader = new JsonReader.Reader(script);
            return new Job(jsonReader, this.config);
        }
    }
}