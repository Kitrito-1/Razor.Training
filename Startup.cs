using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Razor.Training
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();

            //var employees = new Models.OrgViewModel().GetEmployeeData();

            //int i = 0;
            //foreach (var emp in employees)
            //{
            //    if (i == 0)
            //    {
            //        emp.ChangeItems.Add(new Models.EmpChangeItem() { Id = Guid.NewGuid(), Eid = emp.Id, ChgDate = DateTime.Now, ChgField = "職位", OldValue = "副總經理", NewValue = "總經理" });
            //        emp.ChangeItems.Add(new Models.EmpChangeItem() { Id = Guid.NewGuid(), Eid = emp.Id, ChgDate = DateTime.Now, ChgField = "電話", OldValue = "07777777", NewValue = "0888888" });
            //    }
            //    if (i == 1)
            //    {
            //        emp.ChangeItems.Add(new Models.EmpChangeItem() { Id = Guid.NewGuid(), Eid = emp.Id, ChgDate = DateTime.Now, ChgField = "血型", OldValue = "B", NewValue = "C" });
            //    }
            //    if (i == 2)
            //    {
            //        emp.ChangeItems.Add(new Models.EmpChangeItem() { Id = Guid.NewGuid(), Eid = emp.Id, ChgDate = DateTime.Now, ChgField = "在職狀態", OldValue = "在職", NewValue = "離職" });
            //    }
            //    if (i == 3)
            //    {
            //        emp.ChangeItems.Add(new Models.EmpChangeItem() { Id = Guid.NewGuid(), Eid = emp.Id, ChgDate = DateTime.Now, ChgField = "姓名", OldValue = "A", NewValue = "B" });
            //    }
            //    i++;
            //}
            //System.IO.File.WriteAllText(@"data.json",JsonConvert.SerializeObject(employees));

            var employees = JsonConvert.DeserializeObject<List<Models.Employee>>(System.IO.File.ReadAllText(@"data.json"));

            services.AddSingleton(employees);
            //services.AddScoped(typeof(List<Models.Employee>));
            //services.AddTransient(typeof(List<Models.Employee>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
