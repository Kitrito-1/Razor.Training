using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections;

namespace Razor.Training
{
    public class multieditformModel : PageModel
    {        
        public List<Models.Employee> Employees { get; set; }

        public String ValidateMessage { get; set; }

        public void OnGet()
        {
            Employees = new Models.OrgViewModel().GetEmployeeData();
            Employees.Add(new Models.Employee());
        }

        public IActionResult OnGetDeleteOne(Guid eid)
        {
            if (eid != Guid.Empty)
            {
                new Models.OrgViewModel().DeleteEmployeeData(eid);
            }

            return RedirectToPage();
        }

        public IActionResult OnPostSave1(List<Models.Employee> employees)
        {
            if (!Action(employees))
            {
                Employees = employees;
                return Page();
            }

            return RedirectToPage();
        }

        public IActionResult OnPostSave2(Dictionary<int, Models.Employee> employees)
        {
            if (!Action(employees.Values.ToList()))
            {
                Employees = employees.Values.ToList();
                return Page();
            }

            return RedirectToPage();
        }

        public IActionResult OnPostDeleteBatch(List<Models.Employee> employees)
        {
            foreach (var emp in employees.Where(x => x.Check))
            {
                new Models.OrgViewModel().DeleteEmployeeData(emp.Id);
            }

            return RedirectToPage();
        }

        public void OnPostEnabledAccount(List<Models.Employee> employees)
        {
            Employees = employees;
        }

        private bool Action(List<Models.Employee> employees)
        {
            if (!ModelState.IsValid)
            {
                foreach(var e1 in ModelState)
                {
                    if(!e1.Key.Contains((employees.Count - 1).ToString()))
                    {
                        if (e1.Value.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
                        {
                            ValidateMessage = "輸入資料有誤";
                            
                        }
                    }
                }

                if(!String.IsNullOrEmpty(ValidateMessage))
                    return false;
            }

            foreach (var emp in employees)
            {
                if (emp.Id != Guid.Empty)
                    new Models.OrgViewModel().UpdateEmployeeData(emp);
                else
                {
                    if(!String.IsNullOrEmpty(emp.Name) && !String.IsNullOrEmpty(emp.Empno))
                    new Models.OrgViewModel().InsertEmployeeData(emp);
                }
            }
            return true;            
        }
    }
}
