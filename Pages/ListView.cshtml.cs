using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;
using System.ComponentModel.DataAnnotations;

namespace Razor.Training
{
    public class ListViewModel : PageModel
    {
        public List<Models.Employee> Employees { get; set; }

        public List<Models.Position> Positions { get; set; }

        [BindProperty]
        [Display(Name = "√ˆ¡‰¶r")]
        public String Search { get; set; }

        public String Message { get; set; }

        public async Task<IActionResult> OnGet()
        {
            await LoadData();

            TempData["Text"] = "Merry Christmas";

            return Page();
        }

        public async Task<IActionResult> OnPostQuery()
        {
            await LoadData();

            if (!String.IsNullOrEmpty(Search))
                Employees = Employees.Where(x => x.Empno.StartsWith(Search)).ToList();

            return Page();
        }

        public IActionResult OnPostUpdateForTable(Guid eid,String name)
        {
            new Models.OrgViewModel().UpdateEmployeeData(new Models.Employee() { Id = eid, Name = name});

            return RedirectToPage();
        }

        public IActionResult OnPostUpdate(Models.Employee data)
        {
            new Models.OrgViewModel().UpdateEmployeeData(data);

            return RedirectToPage();
        }

        private async Task<IActionResult> LoadData()
        {
            using (var client = new HttpClient() { BaseAddress = new Uri(Request.Scheme + "://" + Request.Host.Value) })
            {
                var result = await client.GetAsync("/api/empinfo");

                if (result.IsSuccessStatusCode)
                {
                    Employees = JsonConvert.DeserializeObject<List<Models.Employee>>(await result.Content.ReadAsStringAsync());
                    Positions = new Models.OrgViewModel().GetPositionData();
                }
                else
                {
                    return RedirectToPage("/Error");
                }
            }
            return Page();
        }
    }
}
