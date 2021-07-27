using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AspSample.App.Main.Models;

namespace AspSample.App.Main
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
            services.AddControllers();
            services.AddSpaStaticFiles(configuration =>
                configuration.RootPath = Configuration["ClientRootPath"]
            );

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite()
            );

            services
                .AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = "Firebase";
                    options.DefaultAuthenticateScheme = "Firebase";
                    options.DefaultChallengeScheme = "Firebase";
                })
                .AddScheme<FirebaseAuthenticationSchemeOptions, FirebaseAuthenticationHandler>("Firebase", options => {});

            services.AddAuthorization(options =>
            {
                // options.AddPolicy("Admin", policy =>
                // {
                //     policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                //     policy.RequireAuthenticatedUser();
                //     policy.RequireRole("Admin");
                // });
                // options.AddPolicy("Normal", policy =>
                // {
                //     policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                //     policy.RequireAuthenticatedUser();
                //     policy.RequireRole("Admin", "Normal");
                // });
                // options.AddPolicy("Anonymous", policy =>
                // {
                //     policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                //     policy.RequireAuthenticatedUser();
                //     policy.RequireRole("Admin", "Normal", "Anonymous");
                // });
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
                app.UseExceptionHandler("/api/error/error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = Configuration["ClientSourcePath"];

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer(Configuration["DevServerUri"]);
                }
            });
        }
    }
}
