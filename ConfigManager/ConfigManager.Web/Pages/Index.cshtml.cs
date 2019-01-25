using System.Collections.Generic;
using System.Linq;
using ConfigManager.Core.Contracts;
using ConfigManager.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConfigManager.Web.Pages
{
    public class IndexModel : PageModel
    {
        private IConfigurationReader _configurationReader;

        public IndexModel(IConfigurationReader configurationReader)
        {
            _configurationReader = configurationReader;
        }

        public List<Configuration> ConfigurationList { get; set; }

        public string CurrentFilter { get; set; }
        public string CacheSearchResult { get; set; }
        
        public void OnGet(string searchString)
        {
            CurrentFilter = searchString;

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                CacheSearchResult = _configurationReader.GetValue<string>(searchString);
                ConfigurationList = _configurationReader.SearchByName(searchString).Select(a => new Configuration
                {
                    Type = a.Type,
                    Value = a.Value,
                    IsActive = a.IsActive,
                    Name = a.Name,
                    Id = a.Id
                }).ToList();
            }
            else
            {
                ConfigurationList = _configurationReader.GetAll().Select(a => new Configuration
                {
                    Type = a.Type,
                    Value = a.Value,
                    IsActive = a.IsActive,
                    Name = a.Name,
                    Id = a.Id
                }).ToList();
            }
        }
    }
}
