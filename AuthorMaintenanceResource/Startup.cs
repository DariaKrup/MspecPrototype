namespace AuthorMaintenanceResource
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AuthorMaintenanceResource.DBContext;
    using AuthorMaintenanceResource.Entities;
    using AuthorMaintenanceResource.Extensions;
    using AuthorMaintenanceResource.Models;
    using Microsoft.AspNet.OData.Builder;
    using Microsoft.AspNet.OData.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Newtonsoft.Json.Serialization;

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
            services.AddMvcCore()
               .AddAuthorization()
               //.AddJsonFormatters()
               .AddCors(options =>
               {
                   options.AddPolicy("AllowSpecificOrigin",
                       builder => builder.WithOrigins("http://localhost:4200").AllowAnyHeader()
                       .AllowAnyMethod());
               });

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:5555";
                    options.RequireHttpsMetadata = false;

                    options.ApiName = "api1";
                });

            services.AddDbContext<AuthorDBContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDb");
            });

            services.AddOData();

            services.AddMvc(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
                setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
            })
             .AddJsonOptions(options =>
             {
                 options.SerializerSettings.ContractResolver =
                 new CamelCasePropertyNamesContractResolver();
             });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseCors("AllowSpecificOrigin");

            var builder = new ODataConventionModelBuilder(app.ApplicationServices);
            builder.EntitySet<Author>(nameof(Author));
            builder.EntitySet<Book>(nameof(Book));
            builder.EnableLowerCamelCase();

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Author, AuthorDto>()
                  .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                  $"{src.FirstName} {src.LastName}"))
                  .ForMember(dest => dest.Age, opt => opt.MapFrom(src =>
                  src.DateOfBirth.GetCurrentAge()));

                cfg.CreateMap<Book, BookDto>();
            });

            app.UseMvc(routeBuilder =>
            {
                routeBuilder.Count().Filter().OrderBy().Expand().Select().MaxTop(null);
                routeBuilder.MapODataServiceRoute("ODataRoute", "odata", builder.GetEdmModel());
                routeBuilder.EnableDependencyInjection();
            });
        }
    }
}
