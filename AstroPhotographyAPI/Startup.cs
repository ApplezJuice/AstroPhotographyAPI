﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using AstroPhotographyAPI.Models;
using Microsoft.EntityFrameworkCore.Sqlite;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Microsoft.Extensions.Options;

namespace AstroPhotographyAPI
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
                        builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
                //WithOrigins("http://localhost:1337"
            });

            services.Configure<IPWhitelistConfiguration>(
                this._config.GetSection("IPAddressWhitelistConfiguration"));
            services.AddSingleton<IIPWhitelistConfiguration>(
                resolver => resolver.GetRequiredService<IOptions<IPWhitelistConfiguration>>().Value);

            services.AddScoped<AuthorizeIPAddress>();

            //services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(_config.GetConnectionString("PhotoDBConnection")));
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlite("Data Source=Photos.db"));

            services.AddMvc().AddXmlDataContractSerializerFormatters();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AstroPhotography API", Version = "v1" });
            });

            services.AddScoped<IPhoto, SQLPhotoRepository>();

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, AppDbContext dbcontext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            dbcontext.Database.EnsureCreated();

            app.UseCors("AllowOrigin");

            app.UseStaticFiles();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mikes AstroPhotography API");
                c.RoutePrefix = string.Empty;
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
