using System.Threading.Tasks;
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

        private IConfigurationReader _configurationReader;

        public ConfigurationModel(IConfigurationReader configurationReader)
        {
            _configurationReader = configurationReader;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // return page  
            }

            await _configurationReader.AddAsync(new AddConfigurationDTO
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