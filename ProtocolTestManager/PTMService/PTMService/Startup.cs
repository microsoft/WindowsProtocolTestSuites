// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Protocols.TestManager.PTMService.Abstractions;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Database;
using Microsoft.Protocols.TestManager.PTMService.Database;
using Microsoft.Protocols.TestManager.PTMService.PTMKernelService;
using Microsoft.Protocols.TestManager.PTMService.PTMService.Configurations;
using Microsoft.Protocols.TestManager.PTMService.Storage;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Microsoft.Protocols.TestManager.PTMService.PTMService
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
            services
                .AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;

                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath);
            });

            var PTMServiceStorageRoot = Configuration.GetSection("PTMServiceStorageRoot").Get<string>();
            var connectionString = Configuration.GetConnectionString("Database");
            if (string.IsNullOrEmpty(PTMServiceStorageRoot))
            {
                // Use the app's directory as the default; useful for debugging purposes
                var builder = new SqliteConnectionStringBuilder(connectionString);
                builder.DataSource = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, builder.DataSource));
                connectionString = builder.ToString();
            }

            services.AddPTMServiceDbContext(connectionString);

            services.AddRepositoryPool();

            services.AddSingleton<IScopedServiceFactory<IRepositoryPool>, ScopedServiceFactory>();

            services.Configure<StoragePoolOptions>(options =>
            {
                var storageRoot = Configuration.GetSection("PTMServiceStorageRoot").Get<string>();
                var items = new string[]
                {
                    KnownStorageNodeNames.TestSuite,
                    KnownStorageNodeNames.Configuration,
                    KnownStorageNodeNames.TestResult
                }.Select(name => new KnownStorageNodeItem { Name = name, Path = Path.Combine(storageRoot, name) });

                options.Nodes = items.ToDictionary(item => item.Name, item => item.Path);
            });

            services.AddStoragePool();

            services.Configure<PTMKernelServiceOptions>(options =>
            {
                options.TestEnginePath = Configuration.GetSection("TestEnginePath").Value;
            });

            services.AddPTMKernelService();

            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = int.MaxValue;
            });

            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartHeadersLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
            });
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
