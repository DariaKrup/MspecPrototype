namespace AuthorMaintenanceResource
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using AuthorMaintenanceResource.DBContext;
    using AuthorMaintenanceResource.Entities;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetService<AuthorDBContext>();
                    AddTestData(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
        }

        private static void AddTestData(AuthorDBContext context)
        {
            CreateAuthors(context);
        }

        private static void CreateAuthors(AuthorDBContext context)
        {
            var author = new Author
            {
                Id = Guid.NewGuid(),
                DateOfBirth = DateTime.Now.AddYears(-30),
                FirstName = "Jane",
                LastName = "Seymour",
                Genre = "Female",
                Books = new List<Book>
               {
                  new Book
                  {
                      Title = "Death in Venice",
                      Description = "Fictional Romance"
                  },
                   new Book
                  {
                      Title = "How to solve a crime",
                      Description = "Fictional"
                  }
               }
            };

            context.Add(author);

            var author2 = new Author
            {
                Id = Guid.NewGuid(),
                DateOfBirth = DateTime.Now.AddYears(-40),
                FirstName = "Tom",
                LastName = "Clancy",
                Genre = "Male",
                Books = new List<Book>
               {
                  new Book
                  {
                      Title = "Red",
                      Description = "Fictional"
                  },
                   new Book
                  {
                      Title = "Patriots Games",
                      Description = "Fictional"
                  }
               }
            };

            context.Add(author2);

            context.SaveChanges();
        }
    }
}
