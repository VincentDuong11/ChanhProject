using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChanhProject
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
            });

            SqliteConnection conn = Database.GetConnection();

            using (var transaction = conn.BeginTransaction())
            {
                var createTableCmd = conn.CreateCommand();
                createTableCmd.Transaction = transaction;
                createTableCmd.CommandText =  "CREATE TABLE IF NOT EXISTS books (id INTEGER PRIMARY KEY, title TEXT NOT NULL);" +
                    "CREATE TABLE IF NOT EXISTS authors (id INTEGER PRIMARY KEY, title TEXT NOT NULL) ;" +
                    "CREATE TABLE IF NOT EXISTS events (id INTEGER PRIMARY KEY, event_type INTEGER NOT NULL, user_id INTEGER NOT NULL , " +
                        "book_id INTEGER NOT NULL, timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL );" +
                    "CREATE TABLE IF NOT EXISTS users (id INTEGER PRIMARY KEY, name TEXT NOT NULL);" +
                    "" +
                    "" +
                    "" +
                    "INSERT OR REPLACE INTO books (title) VALUES ('thang')";
                createTableCmd.ExecuteNonQuery();
                transaction.Commit();
            }
        }
    }
}
