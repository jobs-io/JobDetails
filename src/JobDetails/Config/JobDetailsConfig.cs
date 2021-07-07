namespace JobDetails.Config
{
    public class JobDetailsConfig
    {
        public readonly string Source;
        public readonly JobConfig Job;

        public JobDetailsConfig(string json)
        {
            var jsonReader = new JsonReader.Reader(json);
            this.Source = jsonReader.GetString(new string[] { "source" });
            this.Job = new JobConfig(jsonReader);
        }

    }
}