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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Controllers;

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
                    services.AddEndpointsApiExplorer();
                    services.AddSwaggerGen(c =>
                    {
                        c.SwaggerDoc("v1", new() { Title = "First API", Version = "v1" });
                        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                        {
                            Description = "JWT Auth header using bearer scheme. Ex: 'Authorization: Bearer {token}'",
                            Name = "Authorization",
                            In = ParameterLocation.Header,
                            Type = SecuritySchemeType.ApiKey,
                            Scheme = "Bearer"
                        });

                        c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    },
                                    Scheme = "oauth2",
                                    Name = "Bearer",
                                    In = ParameterLocation.Header,
                                },
                                new List<string>()
                            }
                        });
                    });

                    // not sure!
                    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = hostContext.Configuration["Jwt:Issuer"],
                            ValidAudience = hostContext.Configuration["Jwt:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(hostContext.Configuration["Jwt:Key"])),
                            ClockSkew = TimeSpan.Zero
                        };
                    });


                    services.AddScoped<IRepository<Trade>, Repository<Trade>>();
                    services.AddScoped<IRepository<Rule>, Repository<Rule>>();
                    services.AddScoped<IRepository<Rating>, Repository<Rating>>();
                    services.AddScoped<IRepository<CurvePoint>, Repository<CurvePoint>>();
                    services.AddScoped<IRepository<Bid>, Repository<Bid>>();
                    services.AddScoped<IUserRepository, UserRepository>();
                    services.AddScoped<IUserService, UserService>();
                    services.AddScoped<ITradeService, TradeService>();
                    services.AddScoped<IRuleService, RuleService>();
                    services.AddScoped<IRatingService, RatingService>();
                    services.AddScoped<ICurvePointService, CurvePointService>();
                    services.AddScoped<IBidService, BidService>();
                    services.AddScoped<TokenService>();
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
