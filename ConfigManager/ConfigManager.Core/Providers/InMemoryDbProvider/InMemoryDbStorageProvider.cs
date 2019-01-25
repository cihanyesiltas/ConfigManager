using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConfigManager.Core.Contracts;
using ConfigManager.Core.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ConfigManager.Core.Providers.InMemoryDbProvider
{
    internal class InMemoryDbStorageProvider : IStorageProvider
    {
        private const string DbName = "ConfigDb";

        private DbContextOptions<ConfigurationContext> GetConfigurationContextOptions()
        {
            return new DbContextOptionsBuilder<ConfigurationContext>()
                .UseInMemoryDatabase(DbName)
                .Options;
        }

        public bool Exists(string key, string applicationName)
        {
            using (var context = new ConfigurationContext(GetConfigurationContextOptions()))
            {
                return context.Configurations.AsQueryable()
                    .Any(a => a.Name == key && a.ApplicationName == applicationName);
            }
        }

        public ConfigurationDTO Get(string id)
        {
            using (var context = new ConfigurationContext(GetConfigurationContextOptions()))
            {
                var entity = context.Configurations.AsQueryable().FirstOrDefault(a => a.Id == int.Parse(id));
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
        }

        public bool Add(AddStorageConfigurationDTO dto)
        {
            using (var context = new ConfigurationContext(GetConfigurationContextOptions()))
            {
                context.Configurations.Add(new Configuration
                {
                    Type = dto.Type,
                    Value = dto.Value,
                    Name = dto.Name,
                    IsActive = dto.IsActive,
                    CreationDate = DateTime.Now,
                    LastModifyDate = DateTime.Now,
                    ApplicationName = dto.ApplicationName
                });

                context.SaveChanges();
                return true;
            }
        }

        public async Task<bool> AddAsync(AddStorageConfigurationDTO dto)
        {
            using (var context = new ConfigurationContext(GetConfigurationContextOptions()))
            {
                await context.Configurations.AddAsync(new Configuration
                {
                    Type = dto.Type,
                    Value = dto.Value,
                    Name = dto.Name,
                    IsActive = dto.IsActive,
                    CreationDate = DateTime.Now,
                    LastModifyDate = DateTime.Now,
                    ApplicationName = dto.ApplicationName
                });

                await context.SaveChangesAsync();

                return true;
            }
        }

        public bool Update(UpdateConfigurationDTO dto)
        {
            using (var context = new ConfigurationContext(GetConfigurationContextOptions()))
            {
                var entity = context.Configurations.AsQueryable().FirstOrDefault(a => a.Id == int.Parse(dto.Id));
                if (entity != null)
                {
                    entity.LastModifyDate = DateTime.Now;
                    entity.Value = dto.Value;
                    entity.Type = dto.Type;
                    entity.IsActive = dto.IsActive;

                    context.SaveChanges();

                    return true;
                }

                return false;
            }
        }

        public List<ConfigurationDTO> GetList(string applicationName)
        {
            using (var context = new ConfigurationContext(GetConfigurationContextOptions()))
            {
                return context.Configurations.AsQueryable()
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
        }

        public List<ConfigurationDTO> Search(string searchName, string applicationName)
        {
            using (var context = new ConfigurationContext(GetConfigurationContextOptions()))
            {
                return context.Configurations.AsQueryable()
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
        }

        public List<ConfigurationDTO> GetList(string applicationName, DateTime lastModifyDate)
        {
            using (var context = new ConfigurationContext(GetConfigurationContextOptions()))
            {
                return context.Configurations.AsQueryable()
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
            }
        }
    }
}