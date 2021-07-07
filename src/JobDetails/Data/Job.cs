using JobDetails.Config;

namespace JobDetails.Data
{
    public class Job
    {
        public readonly string Title;
        public readonly string Company;
        public readonly string Description;

        public Job(JsonReader.Reader reader, JobDetailsConfig config)
        {
            Title = reader.GetString(config.Job.Title);
            Company = reader.GetString(config.Job.Company);
            Description = reader.GetString(config.Job.Description);
        }
    }
}