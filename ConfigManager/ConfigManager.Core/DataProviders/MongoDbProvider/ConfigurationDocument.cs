using System;
using MongoDB.Bson;

namespace ConfigManager.Core.DataProviders.MongoDbProvider
{
   internal class ConfigurationDocument
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }
        public string ApplicationName { get; set; }
        public DateTime LastModifyDate { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
