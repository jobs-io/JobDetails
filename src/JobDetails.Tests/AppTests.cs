using NUnit.Framework;
using System.Threading.Tasks;
using System.IO;
using JobDetails.Config;
using JobDetails.Data;
using Moq;
using JobDetails.Core;
using System.Net.Http;
using System.Collections.Generic;

namespace JobDetails.Tests
{

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
            var job = await new App(config, dataStoreMock.Object, new HttpClient()).GetJob();

            Assert.AreEqual(testConfig.Title, job.Title);
            Assert.AreEqual(testConfig.Company, job.Company);
            Assert.AreEqual(testConfig.Description, job.Description);
            Assert.AreEqual(config.Source, job.Source);
        }

        [Test]
        public async Task ShouldCreateJobIfItDoesNotExist() {
            var httpClientMock = new Mock<IHttpClient>();
            var response = new HttpResponseMessage() {StatusCode = System.Net.HttpStatusCode.OK, Content = new StringContent(testConfig.Content) };
            httpClientMock.Setup(x => x.GetAsync(config.Source)).Returns(Task.FromResult(response));
            this.dataStoreMock.Setup(x => x.JobExists(config.Source)).Returns(false);
            this.dataStoreMock.Setup(x => x.CreateJob(config.Source, testConfig.Content));

            var job = await new App(config, dataStoreMock.Object, httpClientMock.Object).GetJob();

            this.dataStoreMock.Verify(x => x.JobExists(config.Source));
            this.dataStoreMock.Verify(x => x.CreateJob(config.Source, testConfig.Content));
        }

        [Test]
        public async Task ShouldGetJobIfItDoesExist() {
            var httpClientMock = new Mock<IHttpClient>();
            httpClientMock.Setup(x => x.GetAsync(config.Source));
            this.dataStoreMock.Setup(x => x.JobExists(config.Source)).Returns(true);
            this.dataStoreMock.Setup(x => x.GetJob(config.Source)).Returns(new Dictionary<string, string>() { { config.Source, testConfig.Content } });
            this.dataStoreMock.Setup(x => x.CreateJob(config.Source, testConfig.Content));

            var job = await new App(config, dataStoreMock.Object, httpClientMock.Object).GetJob();

            this.dataStoreMock.Verify(x => x.JobExists(config.Source));
            this.dataStoreMock.Verify(x => x.GetJob(config.Source));
            this.dataStoreMock.Verify(x => x.CreateJob(config.Source, testConfig.Content), Times.Never());
            httpClientMock.Verify(x => x.GetAsync(config.Source), Times.Never());
            Assert.AreEqual(testConfig.Title, job.Title);
        }
    }
}