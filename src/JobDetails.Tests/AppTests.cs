using NUnit.Framework;
using System.Threading.Tasks;
using System.IO;
using JobDetails.Config;
using JobDetails.Data;
using Moq;

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
        private Mock<IDataStore> dataStoreMock;

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
            this.dataStoreMock = new Mock<IDataStore>();
        }

        [Test]
        public async Task ShouldGetJobDetails()
        {
            var job = await new App(config, dataStoreMock.Object).GetJob();

            Assert.AreEqual(testConfig.Title, job.Title);
            Assert.AreEqual(testConfig.Company, job.Company);
            Assert.AreEqual(testConfig.Description, job.Description);
            Assert.AreEqual(config.Source, job.Source);
        }

        [Test]
        public async Task ShouldCreateJobIfItDoesNotExist() {
            this.dataStoreMock.Setup(x => x.JobExists(config.Source)).Returns(false);

            var job = await new App(config, dataStoreMock.Object).GetJob();

            this.dataStoreMock.Verify(x => x.JobExists(config.Source));
            // Assert.AreEqual(testConfig.Title, job.Title);
            // Assert.AreEqual(testConfig.Company, job.Company);
            // Assert.AreEqual(testConfig.Description, job.Description);
            // Assert.AreEqual(config.Source, job.Source);

        }

        [Test]
        public async Task ShouldGetJobIfItDoesExist() {}
    }
}