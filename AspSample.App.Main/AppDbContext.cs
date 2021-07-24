using System;
using System.Data.SQLite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using AspSample.App.Main.Models;

namespace AspSample.App.Main
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }

        private IConfiguration Configuration { get; }

        public AppDbContext(IConfiguration configuration)
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
