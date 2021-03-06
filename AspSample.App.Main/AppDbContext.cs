using System.Data.SQLite;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using AspSample.App.Main.Models;

namespace AspSample.App.Main
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<Post> Posts { get; set; }

        private IConfiguration Configuration { get; }

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var builder = new SQLiteConnectionStringBuilder()
            {
                DataSource = Configuration["SQLiteDataSource"],
                ForeignKeys = true
            };
            options.UseSqlite(builder.ConnectionString);
        }
    }
}
