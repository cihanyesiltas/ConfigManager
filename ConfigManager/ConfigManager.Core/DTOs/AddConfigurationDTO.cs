namespace ConfigManager.Core.DTOs
{
    public class AddConfigurationDTO
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }
    }
}
