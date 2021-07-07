using NUnit.Framework;
using System.Threading.Tasks;
using System.IO;
using JobDetails.Config;

namespace JobDetails.Tests
{
    public class TestConfig
    {
        public readonly string Title;
        public readonly string Company;
        public readonly string Description;

        public TestConfig(string json)
        {
            var jsonReader = new JsonReader.Reader(json);
            Title = jsonReader.GetString(new string[] { "title" });
            Company = jsonReader.GetString(new string[] { "company" });
            Description = jsonReader.GetString(new string[] { "description" });
        }
    }

    public class AppTests
    {
        private JobDetailsConfig config;
        private TestConfig testConfig;

        [SetUp]
        public void Setup()
        {
            using (var reader = new StreamReader("../../../app-config.json"))
            {
                config = new JobDetailsConfig(reader.ReadToEnd());
            }
            using (var reader = new StreamReader("../../../test-config.json"))
            {
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