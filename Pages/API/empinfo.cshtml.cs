using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Razor.Training
{
    public class empinfoModel : PageModel
    {
        public IActionResult OnGet()
        {
            return new JsonResult(new Models.OrgViewModel().GetEmployeeData());
        }
    }
}
