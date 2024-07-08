using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Dot.Net.WebApi.Domain;
using Microsoft.Extensions.DependencyInjection;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Services;
using Dot.Net.WebApi.Repositories;

namespace Dot.Net.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddScoped<IRepository<User>, Repository<User>>();
                    services.AddScoped<IUserService, UserService>();
                    string connString = hostContext.Configuration.GetConnectionString("DefaultConnection");
                    services.AddDbContext<LocalDbContext>(options =>
                    {
                        options.UseSqlServer(connString);
                    });
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
