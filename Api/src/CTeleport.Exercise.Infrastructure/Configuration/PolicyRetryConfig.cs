namespace CTeleport.Exercise.Infrastructure.Configuration
{
    internal class PolicyRetryConfig
    {
        public int Wait { get; set; }
        public int Retry { get; set; }
        public int Timeout { get; set; }
    }
}
