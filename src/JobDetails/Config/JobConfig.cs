using JsonReader;

namespace JobDetails.Config
{
    public class JobConfig
    {
        public readonly string ScriptTemplate;
        public readonly string Path;
        public readonly string[] Title;
        public readonly string[] Company;
        public readonly string[] Description;

        public JobConfig(JsonReader.Reader reader)
        {
            this.ScriptTemplate = reader.GetString(new string[] { "job", "scriptTemplate" });
            this.Path = reader.GetString(new string[] { "job", "path" });
            this.Title = reader.GetItems<string>(new string[] { "job", "title" }, new MapStringValue());
            this.Company = reader.GetItems<string>(new string[] { "job", "company" }, new MapStringValue());
            this.Description = reader.GetItems<string>(new string[] { "job", "description" }, new MapStringValue());
        }
    }
}