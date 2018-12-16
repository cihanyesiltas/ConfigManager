using ConfigManager.Core.Contracts;
using ConfigManager.Core.DTOs;
using ConfigManager.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConfigManager.Web.Pages
{
    public class EditConfigurationModel : PageModel
    {
        private IConfigurationReader _configurationReader;

        public EditConfigurationModel(IConfigurationReader configurationReader)
        {
            _configurationReader = configurationReader;
        }


        [BindProperty]
        public Configuration Configuration { get; set; }

        public void OnGet(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                var configuration = _configurationReader.GetById(id);

                Configuration = new Configuration
                {
                    Id = configuration.Id,
                    Type = configuration.Type,
                    Value = configuration.Value,
                    IsActive = configuration.IsActive,
                    Name = configuration.Name
                };
            }
        }

        public ActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _configurationReader.Update(new UpdateConfigurationDTO
            {
                Id = Configuration.Id,
                Type = Configuration.Type,
                Value = Configuration.Value,
                IsActive = Configuration.IsActive
            });

            return RedirectToPage("Index");
        }
    }
}