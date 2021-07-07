// using System;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using JsonReader;
// using System.Text.Json;
using System.IO;
// using HtmlReader;
// using JsonReader;

namespace JobDetails.Tests
{
    public class Job {
        public string Title { get; set; }
        public string Company { get; set; }
        public string Description { get; set; }

        public Job(JsonReader.Reader reader, JobDetailsConfig config)
        {
            Title = reader.GetString(config.Job.Title);
            Company = reader.GetString(config.Job.Company);
            Description = reader.GetString(config.Job.Description);
        }
    }

    public class JobConfig {
        public readonly string ScriptTemplate;
        public readonly string Path;
        public readonly string[] Title;
        public readonly string[] Company;
        public readonly string[] Description;

        public JobConfig(JsonReader.Reader reader) {
            this.ScriptTemplate = reader.GetString(new string[] { "job", "scriptTemplate" });
            this.Path = reader.GetString(new string[] {"job", "path"});
            this.Title = reader.GetItems<string>(new string[] {"job", "title"}, new MapStringValue());
            this.Company = reader.GetItems<string>(new string[] {"job", "company"}, new MapStringValue());
            this.Description = reader.GetItems<string>(new string[] {"job", "description"}, new MapStringValue());
        }
    }

    public class JobDetailsConfig {
        public readonly string Source;
        public readonly JobConfig Job;

        public JobDetailsConfig(string json)
        {
            var jsonReader = new JsonReader.Reader(json);
            this.Source = jsonReader.GetString(new string[] {"source"});
            this.Job = new JobConfig(jsonReader);
        }

    }

        public class App {
            private readonly JobDetailsConfig config;
            public App(JobDetailsConfig config) {
                this.config = config;
            }

            public async ValueTask<Job> GetJob() {
                var httpClient = new HttpClient();
                var results = await httpClient.GetAsync(config.Source);

                Assert.AreEqual(HttpStatusCode.OK, results.StatusCode);

                var html = await results.Content.ReadAsStringAsync();
                var htmlReader = new HtmlReader.Reader(html);
                var scriptTemplate = config.Job.ScriptTemplate;
                var script = htmlReader.ParseScript(config.Job.Path, scriptTemplate);

                var jsonReader = new JsonReader.Reader(script);
                return new Job(jsonReader, config);
            }
        }

    public class TestConfig {
        public readonly string Title;
        public readonly string Company;
        public readonly string Description;

        public TestConfig(string json) {
            var jsonReader = new JsonReader.Reader(json);
            Title =  jsonReader.GetString(new string[] {"title"});
            Company =  jsonReader.GetString(new string[] {"company"});
            Description =  jsonReader.GetString(new string[] {"description"});
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
            var job = await new App(config).GetJob();

            Assert.AreEqual(testConfig.Title, job.Title);
            Assert.AreEqual(testConfig.Company, job.Company);
            Assert.AreEqual(testConfig.Description, job.Description);
        }
    }
}