using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace Razor.Training
{
    public class formModel : PageModel
    {
        //[BindProperty]
        public Employee Employee { get; set; }

        public String Message { get; set; }

        public String Search { get; set; }

        private List<Employee> Employees { get; } = new List<Employee>();

        public IActionResult OnGet(String keyword, String message)
        {
            Search = keyword;
            Message = message;

            if (!String.IsNullOrEmpty(keyword))
            {
                for (int i = 1; i < 11; i++)
                {
                    Employees.Add(new Employee() { Name = "Johnny" + i.ToString().PadLeft(2, '0'), Empno = i.ToString().PadLeft(6, '0'), EnterDate = DateTime.Now.AddDays(i), Email = "johnny" + (i * 10).ToString().PadLeft(2, '0') + ".tasi@doublegreen.com" });
                }

                Employee = Employees.Where(x => x.Name.Contains(keyword) || x.Empno.StartsWith(keyword) || x.Email.Contains(keyword)).FirstOrDefault();
            }
            else
                return RedirectToPage("/Error");

            return Page();
        }

        public void OnPost()
        {

        }

        public void OnPostQuery(String search)
        {
            OnGet(search, "");
        }

        //public void OnPostShow(Employee employee)
        //{
        //    Message = $"收到表單內容：{employee.Name}, {employee.EnterDate.ToShortDateString()}";
        //}

        public async Task<IActionResult> OnPostShow(Employee employee)
        {
            await Task.Run(() =>
            {
                int j = 0;
                for (int i = 0; i < 999; i++)
                {
                    if (i >= 900)
                    {
                        if (!String.IsNullOrEmpty(Message))
                            Message += " , ";
                        Message += i.ToString().PadLeft(3, '0');
                        j = i;
                    }
                }
            });

            if (!String.IsNullOrEmpty(Message))
                Message += " , ";
            Message += $"收到表單內容：{employee.Name}, {employee.EnterDate.ToShortDateString()}";

            return Page();
        }

        public IActionResult OnPostClear(Employee employee)
        {
            return RedirectToPage("/form", new { keyword = "x", message = $"收到表單內容：{employee.Name}, {employee.EnterDate.ToShortDateString()}" });
        }

        public override Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            return base.OnPageHandlerSelectionAsync(context);
        }

        public override void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
            base.OnPageHandlerSelected(context);
        }

        public override Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            return base.OnPageHandlerExecutionAsync(context, next);
        }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            base.OnPageHandlerExecuting(context);
        }

        public override void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            base.OnPageHandlerExecuted(context);
        }
    }
}
