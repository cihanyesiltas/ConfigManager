using ConfigManager.Core.Contracts;
using ConfigManager.Core.DTOs;
using ConfigManager.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConfigManager.Web.Pages
{
    public class ConfigurationModel : PageModel
    {
        [BindProperty]
        public Configuration Configuration { get; set; }

        private IConfigurationReader _configurationManager;

        public ConfigurationModel(IConfigurationReader configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public void OnGet()
        {
        }

        public ActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // return page  
            }
        
            _configurationManager.Add(new AddConfigurationDTO
            {
                Type = Configuration.Type,
                Value = Configuration.Value,
                Name = Configuration.Name,
                IsActive = Configuration.IsActive
            });

            return RedirectToPage("Index");
        }
    }
}