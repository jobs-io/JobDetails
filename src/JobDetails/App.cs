using System.Net.Http;
using System.Threading.Tasks;
using JobDetails.Config;
using JobDetails.Data;

namespace JobDetails
{
    public class App
    {
        private readonly JobDetailsConfig config;
        public App(JobDetailsConfig config)
        {
            this.config = config;
        }

        public async ValueTask<Job> GetJob()
        {
            var httpClient = new HttpClient();
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