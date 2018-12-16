using System;

namespace ConfigManager.Core.DTOs
{
   public class CacheConfigurationDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string ApplicationName { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifyDate { get; set; }
        public bool IsActive { get; set; }
    }
}
