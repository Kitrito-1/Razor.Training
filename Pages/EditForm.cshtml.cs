using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Razor.Training
{
    public class EditFormModel : PageModel
    {
        [BindProperty]
        public Models.Employee Employee { get; set; }

        public String ValidateMessage { get; set; }

        [BindProperty]
        public String InitDate { get => Employee.Init.Item1.HasValue ? Employee.Init.Item1.Value.ToString("yyyy/MM/dd HH:mm:ss") : "" ; set => Employee.Init = (!String.IsNullOrEmpty(value) ? DateTime.Parse(value) : default(DateTime), InitUid); }

        [BindProperty]
        public Guid? InitUid { get => Employee.Init.Item2; set => Employee.Init = (!String.IsNullOrEmpty(InitDate) ? DateTime.Parse(InitDate) : default(DateTime), value); }

        public void OnGet(Guid eid)
        {
            Employee = new Models.OrgViewModel().GetEmployeeData(eid);
        }

        public IActionResult OnPost()
        {
            if(!ModelState.IsValid)
            {
                 foreach(var e1 in ModelState.Values)
                {
                    foreach(var e2 in e1.Errors)
                    {
                        if (!String.IsNullOrEmpty(ValidateMessage))
                            ValidateMessage += "<BR>";
                        ValidateMessage += e2.ErrorMessage;
                    }
                }

                return Page();
            }
            new Models.OrgViewModel().UpdateEmployeeData(Employee);

            return Content(@"<script>window.close();opener.location.reload();</script>", "text/html");
        }
    }
}
