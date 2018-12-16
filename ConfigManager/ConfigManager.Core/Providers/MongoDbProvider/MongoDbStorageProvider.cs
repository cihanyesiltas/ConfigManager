using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConfigManager.Core.Contracts;
using ConfigManager.Core.DTOs;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ConfigManager.Core.Providers.MongoDbProvider
{
    internal class MongoDbStorageProvider : IStorageProvider
    {
        private const string DbName = "ConfigDb";
        private const string CollectionName = "Configuration";
        private readonly MongoClient _dbClient;

        public MongoDbStorageProvider(string connectionString)
        {
            _dbClient = new MongoClient(connectionString);
        }

        private IMongoDatabase Db => _dbClient.GetDatabase(DbName);

        private IMongoCollection<ConfigurationDocument> Collection => Db.GetCollection<ConfigurationDocument>(CollectionName);

        public bool Exists(string key, string applicationName)
        {
            return Collection.AsQueryable().Any(a => a.Name == key && a.ApplicationName == applicationName);
        }

        public ConfigurationDTO Get(string id)
        {
            var entity = Collection.AsQueryable().FirstOrDefault(a => a.Id == ObjectId.Parse(id));
            if (entity != null)
            {
                return new ConfigurationDTO
                {
                    Id = entity.Id.ToString(),
                    IsActive = entity.IsActive,
                    Name = entity.Name,
                    Type = entity.Type,
                    ApplicationName = entity.ApplicationName,
                    Value = entity.Value,
                    CreationDate = entity.CreationDate,
                    LastModifyDate = entity.LastModifyDate
                };
            }

            return null;
        }

        public bool Add(AddStorageConfigurationDTO dto)
        {
            Collection.InsertOne(new ConfigurationDocument
            {
                Type = dto.Type,
                Value = dto.Value,
                Name = dto.Name,
                IsActive = dto.IsActive,
                CreationDate = DateTime.Now,
                LastModifyDate = DateTime.Now,
                ApplicationName = dto.ApplicationName
            });
            return true;
        }

        public async Task<bool> AddAsync(AddStorageConfigurationDTO dto)
        {
            await Collection.InsertOneAsync(new ConfigurationDocument
            {
                Type = dto.Type,
                Value = dto.Value,
                Name = dto.Name,
                IsActive = dto.IsActive,
                CreationDate = DateTime.Now,
                LastModifyDate = DateTime.Now,
                ApplicationName = dto.ApplicationName
            });

            return true;
        }

        public bool Update(UpdateConfigurationDTO dto)
        {
            var filter = Builders<ConfigurationDocument>.Filter.AnyEq("_id", new ObjectId(dto.Id));
            var update = Builders<ConfigurationDocument>.Update.Set("Value", dto.Value).Set("IsActive", dto.IsActive)
                .Set("Type", dto.Type).CurrentDate("LastModifyDate");

            var result = Collection.UpdateOne(filter, update);
            return result.IsAcknowledged ? result.ModifiedCount > 0 : result.IsAcknowledged;
        }

        public List<ConfigurationDTO> GetList(string applicationName)
        {
            return Collection.AsQueryable()
                .Where(a => a.ApplicationName == applicationName).AsEnumerable()
                .Select(a => new ConfigurationDTO
                {
                    Id = a.Id.ToString(),
                    IsActive = a.IsActive,
                    Name = a.Name,
                    Type = a.Type,
                    ApplicationName = a.ApplicationName,
                    Value = a.Value,
                    CreationDate = a.CreationDate,
                    LastModifyDate = a.LastModifyDate
                }).ToList();
        }

        public List<ConfigurationDTO> Search(string searchName, string applicationName)
        {
            return Collection.AsQueryable()
                .Where(a => a.ApplicationName == applicationName
                            && a.Name.ToLower().Contains(searchName.ToLower())).AsEnumerable()
                .Select(a => new ConfigurationDTO
                {
                    Id = a.Id.ToString(),
                    IsActive = a.IsActive,
                    Name = a.Name,
                    Type = a.Type,
                    ApplicationName = a.ApplicationName,
                    Value = a.Value,
                    CreationDate = a.CreationDate,
                    LastModifyDate = a.LastModifyDate
                }).ToList();
        }

        public List<ConfigurationDTO> GetList(string applicationName, DateTime lastModifyDate)
        {
            var result = Collection.AsQueryable()
                .Where(a => a.ApplicationName == applicationName
                            && a.LastModifyDate > lastModifyDate).AsEnumerable()
                .Select(a => new ConfigurationDTO
                {
                    Id = a.Id.ToString(),
                    IsActive = a.IsActive,
                    Name = a.Name,
                    Type = a.Type,
                    ApplicationName = a.ApplicationName,
                    Value = a.Value,
                    CreationDate = a.CreationDate,
                    LastModifyDate = a.LastModifyDate
                }).ToList();

            return result;
        }
    }
}
