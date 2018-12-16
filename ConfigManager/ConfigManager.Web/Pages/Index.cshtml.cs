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
        private IConfigurationReader _configurationManager;

        public IndexModel(IConfigurationReader configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public List<Configuration> ConfigurationList { get; set; }

        public string CurrentFilter { get; set; }

        public void OnGet(string searchString)
        {
            CurrentFilter = searchString;

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                ConfigurationList = _configurationManager.SearchByName(searchString).Select(a => new Configuration
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
                ConfigurationList = _configurationManager.GetAll().Select(a => new Configuration
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
