namespace ConfigManager.Web.Models
{
    public class Configuration
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }
    }
}
