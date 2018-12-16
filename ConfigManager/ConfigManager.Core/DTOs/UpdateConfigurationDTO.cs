namespace ConfigManager.Core.DTOs
{
    public class UpdateConfigurationDTO
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }
    }
}
