using System.Collections.Generic;
using System.Threading.Tasks;
using ConfigManager.Core.DTOs;

namespace ConfigManager.Core.Contracts
{
    public interface IConfigurationReader
    {
        T GetValue<T>(string key);
        bool Add(AddConfigurationDTO dto);
        Task<bool> AddAsync(AddConfigurationDTO dto);
        bool Update(UpdateConfigurationDTO dto);
        List<ConfigurationDTO> GetAll();
        List<ConfigurationDTO> SearchByName(string name);
        ConfigurationDTO GetById(string id);
    }
}
