using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ConfigManager.Core.Contracts;
using ConfigManager.Core.DTOs;
using ConfigManager.Core.Extensions;
using Dapper;
using Npgsql;

namespace ConfigManager.Core.Providers.PostgreSqlProvider
{
    internal class PostgreSqlStorageProvider : IStorageProvider
    {
        private readonly string _connectionString;

        public PostgreSqlStorageProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection Connection => new NpgsqlConnection(_connectionString);

        public T GetValue<T>(string key, string applicationName)
        {
            using (var dbConnection = Connection)
            {
                dbConnection.Open();
                var entity = dbConnection
                    .Query<Configuration>($"SELECT * FROM \"Configuration\" WHERE \"Name\"='{key}' AND \"ApplicationName\" = '{applicationName}' LIMIT 1")
                    .FirstOrDefault();

                if (entity != null)
                {
                    return entity.Value.Cast<T>(entity.Type);
                }

                return default(T);
            }
        }

        public bool Exists(string key, string applicationName)
        {
            using (var dbConnection = Connection)
            {
                dbConnection.Open();

                return dbConnection
                    .Query<Configuration>(
                        $"SELECT * FROM \"Configuration\" WHERE \"Name\"='{key}' AND \"ApplicationName\" = '{applicationName}' LIMIT 1")
                    .Any();
            }
        }

        public ConfigurationDTO Get(string id)
        {
            using (var dbConnection = Connection)
            {
                dbConnection.Open();

                var entity = dbConnection
                    .Query<Configuration>(
                        $"SELECT * FROM \"Configuration\" WHERE \"Id\" = {id} LIMIT 1")
                    .FirstOrDefault();

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
            using (var dbConnection = Connection)
            {
                var query = "INSERT INTO \"Configuration\" (\"Name\", \"Type\", \"Value\",\"IsActive\",\"ApplicationName\",\"CreationDate\", \"LastModifyDate\")"
                                + " VALUES(@Name, @Type, @Value,@IsActive, @ApplicationName,@CreationDate, @LastModifyDate )";
                dbConnection.Open();
                dbConnection.Execute(query, new
                {
                    Value = dto.Value,
                    Name = dto.Name,
                    ApplicationName = dto.ApplicationName,
                    IsActive = dto.IsActive,
                    Type = dto.Type,
                    CreationDate = DateTime.Now,
                    LastModifyDate = DateTime.Now
                });
            }

            return true;
        }

        public bool Update(UpdateConfigurationDTO dto)
        {
            using (var dbConnection = Connection)
            {
                var query = "UPDATE \"Configuration\" SET \"Value\" = @Value,"
                                + " \"Type\" = @Type, \"IsActive\"= @IsActive, \"LastModifyDate\"= @LastModifyDate "
                                + " WHERE \"Id\" = @Id";

                dbConnection.Open();
                dbConnection.Execute(query, new
                {
                    Id = int.Parse(dto.Id),
                    Value = dto.Value,
                    IsActive = dto.IsActive,
                    Type = dto.Type,
                    LastModifyDate = DateTime.Now
                });
            }

            return true;
        }

        public List<ConfigurationDTO> GetList(string applicationName)
        {
            using (var dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection
                    .Query<Configuration>(
                        $"SELECT * FROM \"Configuration\" WHERE \"ApplicationName\" = '{applicationName}'")
                    .Select(a => new ConfigurationDTO
                    {
                        ApplicationName = a.ApplicationName,
                        Type = a.Type,
                        IsActive = a.IsActive,
                        Name = a.Name,
                        CreationDate = a.CreationDate,
                        Value = a.Value,
                        Id = a.Id.ToString(),
                        LastModifyDate = a.LastModifyDate
                    }).ToList();
            }
        }

        public List<ConfigurationDTO> Search(string searchName, string applicationName)
        {
            using (var dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection
                    .Query<Configuration>(
                        $"SELECT * FROM \"Configuration\" WHERE \"ApplicationName\" = '{applicationName}' AND lower(\"Name\") LIKE lower('%{searchName}%')")
                    .Select(a => new ConfigurationDTO
                    {
                        ApplicationName = a.ApplicationName,
                        Type = a.Type,
                        IsActive = a.IsActive,
                        Name = a.Name,
                        CreationDate = a.CreationDate,
                        Value = a.Value,
                        Id = a.Id.ToString(),
                        LastModifyDate = a.LastModifyDate
                    }).ToList();
            }
        }

        public List<ConfigurationDTO> GetList(string applicationName, DateTime lastModifyDate)
        {
            using (var dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection
                    .Query<Configuration>(
                        $"SELECT * FROM \"Configuration\" WHERE \"ApplicationName\" = '{applicationName}' AND \"LastModifyDate\" > '{lastModifyDate:yyyy-MM-dd HH:mm:ss.ffffff}'")
                    .Select(a => new ConfigurationDTO
                    {
                        ApplicationName = a.ApplicationName,
                        Type = a.Type,
                        IsActive = a.IsActive,
                        Name = a.Name,
                        CreationDate = a.CreationDate,
                        Value = a.Value,
                        Id = a.Id.ToString(),
                        LastModifyDate = a.LastModifyDate
                    }).ToList();
            }
        }
    }
}
