namespace JobDetails.Tests
{
    public class TestConfig
    {
        public readonly string Title;
        public readonly string Company;
        public readonly string Description;
        public readonly string Content;

        public TestConfig(string json)
        {
            var jsonReader = new JsonReader.Reader(json);
            Title = jsonReader.GetString(new string[] { "title" });
            Company = jsonReader.GetString(new string[] { "company" });
            Description = jsonReader.GetString(new string[] { "description" });
            Content = jsonReader.GetString(new string[] { "content" });
        }
    }
}