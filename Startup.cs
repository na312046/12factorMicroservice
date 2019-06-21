using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ConsoleApplication.Data;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApplication
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;

        public Startup(IHostingEnvironment env, ILogger<Startup> logger)
        {
            _logger = logger;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables("APP_");
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            // Setup DB
            // string dbHost = Configuration["Database:dbtestserverfor.database.windows.net"];
            // string dbPort = Configuration["Database:1433"];
            // string dbUser = Configuration["Database:User"];
            // string dbPassword = Configuration["Database:Password"];
            // string dbName = Configuration["Database:Name"];
            // string connectionString = string.Format("Host={0};Port={1};Database={2};User ID={3};Password={4}", dbHost, dbPort, dbName, dbUser, dbPassword);
          //  string strconn="Host=postgraserver.postgres.database.azure.com;Port=1433;Database=dbdemo;User ID=narendra;Password=password@222";
          string strconnection="Server=postgraserver.postgres.database.azure.com;Database=MyDataBase;Port=5432;User Id=narendra@postgraserver;Password=password@222;Ssl Mode=Require;CommandTimeout=1000";
          
         // string str=System.Environment.GetEnvironmentVariable("CONNECTION_STRING");
            services.AddDbContext<WidgetsContext>(options => options.UseNpgsql(strconnection));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            // Create DB on startup
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                // On first run in a Docker compose environment the database might not be ready. Retry until ready.
                bool success = false;
                int retriesLeft = 10;
                do
                {
                    try
                    {

                         var context = app.ApplicationServices.GetService<WidgetsContext>();
                         context.Database.SetCommandTimeout(180);
                        //  if (!context.Database.EnsureCreated())
                        
                        context.Database.Migrate();
                      //serviceScope.ServiceProvider.GetService<WidgetsContext>().Database.Migrate(); 
                        success = true;
                    }
                    catch (System.Net.Sockets.SocketException e)
                    {
                        retriesLeft--;
                        _logger.LogWarning("Could not connect to database. Retrying in 2 seconds.");
                        if (retriesLeft > 0)
                        {
                            System.Threading.Thread.Sleep(2000);
                        } else {
                            throw e;
                        }
                    }
                } while (!success);
                _logger.LogInformation("Connected to database.");
            }
        }
    }
}
