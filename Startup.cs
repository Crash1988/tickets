using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AspCoreServer.Data;
using Swashbuckle.AspNetCore.Swagger;
using AspCoreServer.Models;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AspCoreServer
{
  public class Startup
  {

    
    public static void Main(String[] args)
    {
      Startup.BuildWebHost(args).Run();
    }

    public static IWebHost BuildWebHost(String[] args) =>
        WebHost.CreateDefaultBuilder(args)
               .UseKestrel()
               .UseUrls("http://*:8000")
               .UseStartup<Startup>()
               .Build();


    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
      
    }

    public IConfiguration Configuration { get; }


    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      // Add framework services.
      services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

      services.AddIdentity<ApplicationUser, IdentityRole>(config => {
                      config.User.RequireUniqueEmail = true;
                      config.Password.RequireNonAlphanumeric = false;  })
          .AddEntityFrameworkStores<ApplicationDbContext>()
          .AddDefaultTokenProviders();

      //services.AddCors();
      services.AddAuthentication()
          .AddJwtBearer(cfg =>
          {
            cfg.RequireHttpsMetadata = false;
            cfg.SaveToken = true;

            cfg.TokenValidationParameters = new TokenValidationParameters()
            {
              ValidIssuer = Configuration["Tokens:Issuer"],
              ValidAudience = Configuration["Tokens:Issuer"],
              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
            };

          });
      services.AddMvc();
      services.AddNodeServices();

      //var connectionStringBuilder = new Microsoft.Data.Sqlite.SqliteConnectionStringBuilder { DataSource = "spa.db" };///conection string for sqlite
      //var connectionString = connectionStringBuilder.ToString();

      //services.AddDbContext<SpaDbContext>(options =>
      //     options.UseSqlite(connectionString));

      // Register the Swagger generator, defining one or more Swagger documents
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new Info { Title = "Angular 4.0 Universal & ASP.NET Core advanced starter-kit web API", Version = "v1" });
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      app.UseStaticFiles();
      app.UseAuthentication();

      // DbInitializer.Initialize(context);

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
        {
          HotModuleReplacement = true,
          HotModuleReplacementEndpoint = "/dist/__webpack_hmr"
        });
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
          c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        });

        // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.

       /**
        *app.UseCors(builder => builder
          .AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials());
*/
        app.MapWhen(x => !x.Request.Path.Value.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase), builder =>
        {
          builder.UseMvc(routes =>
          {
            routes.MapSpaFallbackRoute(
                name: "spa-fallback",
                defaults: new { controller = "Home", action = "Index" });
          });
        });
      }
      else
      {/*
        app.UseCors(builder => builder
          .AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials());*/

        app.UseMvc(routes =>
        {
          routes.MapRoute(
           name: "default",
           template: "{controller=Home}/{action=Index}/{id?}");

          routes.MapRoute(
           "Sitemap",
           "sitemap.xml",
           new { controller = "Home", action = "SitemapXml" });

          routes.MapSpaFallbackRoute(
            name: "spa-fallback",
            defaults: new { controller = "Home", action = "Index" });

        });
        app.UseExceptionHandler("/Home/Error");
      }
    }
  }
}
