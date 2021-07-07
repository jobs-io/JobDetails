using System;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using JsonReader;
using System.Text.Json;
using System.IO;
// using HtmlReader;
// using JsonReader;

namespace JobDetails.Tests
{

    public class Job {
        public readonly string ScriptTemplate;
        public readonly string Path;
        public readonly string[] Title;
        public readonly string[] Company;

        public Job(JsonReader.Reader reader) {
            this.ScriptTemplate = reader.GetString(new string[] { "job", "scriptTemplate" });
            this.Path = reader.GetString(new string[] {"job", "path"});
            this.Title = reader.GetItems<string>(new string[] {"job", "title"}, new MapStringValue());
            this.Company = reader.GetItems<string>(new string[] {"job", "company"}, new MapStringValue());
        }
    }

    public class JobMapper : IMapValue<Job>
    {
        public Job Map(JsonElement jsonElement)
        {
            throw new NotImplementedException();
        }
    }

    public class JobDetailsConfig {
        public readonly string Source;
        public readonly Job Job;

        public JobDetailsConfig(string json)
        {
            var jsonReader = new JsonReader.Reader(json);
            this.Source = jsonReader.GetString(new string[] {"source"});
            this.Job = new Job(jsonReader);
        }

    }

    public class TestConfig {
        public readonly string Title;
        public readonly string Company;

        public TestConfig(string json) {
            var jsonReader = new JsonReader.Reader(json);
            Title =  jsonReader.GetString(new string[] {"title"});
            Company =  jsonReader.GetString(new string[] {"company"});
        }
    }

    public class AppTests
    {
        private JobDetailsConfig config;
        private TestConfig testConfig;
        
        [SetUp]
        public void Setup()
        {
            using(var reader = new StreamReader("../../../app-config.json")) {
                config = new JobDetailsConfig(reader.ReadToEnd());
            }
            using (var reader = new StreamReader("../../../test-config.json")) {
                testConfig = new TestConfig(reader.ReadToEnd());
            }
        }

        [Test]
        public async Task ShouldGetJobDetails()
        {
            var httpClient = new HttpClient();

            var results = await httpClient.GetAsync(config.Source);

            Assert.AreEqual(HttpStatusCode.OK, results.StatusCode);

            var html = await results.Content.ReadAsStringAsync();
            var htmlReader = new HtmlReader.Reader(html);
            var scriptTemplate = config.Job.ScriptTemplate;
            var script = htmlReader.ParseScript(config.Job.Path, scriptTemplate);

            var jsonReader = new JsonReader.Reader(script);
            
            Assert.AreEqual(testConfig.Title, jsonReader.GetString(config.Job.Title));
            Assert.AreEqual(testConfig.Company, jsonReader.GetString(config.Job.Company));
        }
    }
}