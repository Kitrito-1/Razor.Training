using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;

namespace Razor.Training
{
    public class ListView2Model : PageModel
    {
        public List<Models.Employee> Employees { get; set; }

        List<Models.Employee> EmployeesDb { get; set; }

        public ListView2Model([FromServices]List<Models.Employee> employees)
        {
            if(employees.Count == 0)
            {
                employees.AddRange(JsonConvert.DeserializeObject<List<Models.Employee>>(System.IO.File.ReadAllText(@"data.json")));
            }
            
            EmployeesDb = employees;
        }

        public void OnGet()
        {
            EmployeesDb = (List<Models.Employee>)Request.HttpContext.RequestServices.GetRequiredService(typeof(List<Models.Employee>));
            Employees = EmployeesDb;
        }

        public void OnPost()
        {

        }

        public void OnPostAddEmpno(Guid id)
        {
            var idx = EmployeesDb.FindIndex(x => x.Id.Equals(id));

            if (idx >= 0) 
            {
                EmployeesDb[idx].Empno = (Int64.Parse(EmployeesDb[idx].Empno) + 1).ToString();
            }

            Employees = EmployeesDb;

        }
    }
}
