using System;
using System.Collections.Generic;
using ConfigManager.Core.DTOs;

namespace ConfigManager.Core.Contracts
{
    public interface IStorageProvider
    {
        T GetValue<T>(string key, string applicationName);
        bool Exists(string key, string applicationName);
        ConfigurationDTO Get(string id);
        bool Add(AddStorageConfigurationDTO dto);
        bool Update(UpdateConfigurationDTO dto);
        List<ConfigurationDTO> GetList(string applicationName);
        List<ConfigurationDTO> GetList(string applicationName, DateTime lastModifyDate);
        List<ConfigurationDTO> GetActiveList(string applicationName);
        List<ConfigurationDTO> Search(string searchName, string applicationName);
    }
}
