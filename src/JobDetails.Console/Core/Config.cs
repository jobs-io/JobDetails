namespace JobDetails.Console.Core
{
    public class Config {
        public readonly string JobDetailsPath;
        public readonly string Source;
        public readonly string AppConfigPath;
        public readonly string Action;

        public Config(string[] args)
        {
            this.JobDetailsPath = args[0];
            this.Source = args[1];
            this.AppConfigPath = args[2];
            if (args.Length > 3) 
                this.Action = args[3];
        }
    }
}
