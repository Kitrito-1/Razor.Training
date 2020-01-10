using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;

namespace Razor.Training
{
    public class MasterFormModel : PageModel
    {
        public Models.Employee Employee { get; set; }
        
        List<Models.Employee> EmployeesDb { get; set; }

        public MasterFormModel([FromServices]List<Models.Employee> employees)
        {
            if (employees.Count == 0)
            {
                employees.AddRange(JsonConvert.DeserializeObject<List<Models.Employee>>(System.IO.File.ReadAllText(@"data.json")));
            }

            EmployeesDb = employees;
        }

        public void OnGet(Guid eid)
        {
            EmployeesDb = (List<Models.Employee>)Request.HttpContext.RequestServices.GetRequiredService(typeof(List<Models.Employee>));
            Employee = EmployeesDb.Where(x => x.Id.Equals(eid)).FirstOrDefault();
        }

        public IActionResult OnPostSave(Models.Employee employee,List<Models.EmpChangeItem> items)
        {
            items = items ?? new List<Models.EmpChangeItem>();
            employee = employee ?? new Models.Employee();

            employee.ChangeItems = items;
            Employee = employee;
            
            if (ModelState.IsValid)
            {
                var idx = EmployeesDb.FindIndex(o => o.Id.Equals(employee.Id));

                if (idx >= 0)
                {
                    EmployeesDb[idx] = employee;

                    return RedirectToPage("/listview2");
                }

                return Page();
            }

            return Page();
        }
    }
}
