using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConfigManager.Core.DTOs;

namespace ConfigManager.Core.Contracts
{
    public interface IStorageProvider
    {
        bool Exists(string key, string applicationName);
        ConfigurationDTO Get(string id);
        bool Add(AddStorageConfigurationDTO dto);
        Task<bool> AddAsync(AddStorageConfigurationDTO dto);
        bool Update(UpdateConfigurationDTO dto);
        List<ConfigurationDTO> GetList(string applicationName);
        List<ConfigurationDTO> GetList(string applicationName, DateTime lastModifyDate);
        List<ConfigurationDTO> Search(string searchName, string applicationName);
    }
}
