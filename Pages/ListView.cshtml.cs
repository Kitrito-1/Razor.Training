using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;

namespace Razor.Training
{
    public class ListViewModel : PageModel
    {
        public List<Models.Employee> Employees { get; set; }

        public List<Models.Position> Position { get; set; } 

        public async Task<IActionResult> OnGet()
        {
            using (var client = new HttpClient() { BaseAddress = new Uri(Request.Scheme + "://"+ Request.Host.Value) })
            {
                var result = await client.GetAsync("/api/empinfo");

                if (result.IsSuccessStatusCode)
                {
                    Employees = JsonConvert.DeserializeObject<List<Models.Employee>>(await result.Content.ReadAsStringAsync());
                    Position = new Models.OrgViewModel().GetPositionData();
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
