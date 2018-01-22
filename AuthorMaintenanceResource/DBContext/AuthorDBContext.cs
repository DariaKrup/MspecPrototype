namespace AuthorMaintenanceResource.DBContext
{
    using AuthorMaintenanceResource.Entities;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AuthorDBContext : DbContext
    {
        public AuthorDBContext(DbContextOptions<AuthorDBContext> options)
           : base(options)
        {

        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

    }
}
