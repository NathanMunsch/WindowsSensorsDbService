using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using WindowsSensorsDbService.Data;
using WindowsSensorsDbService.Helpers;
using WindowsSensorsDbService.SignalR;

namespace WindowsSensorsDbService
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddHostedService<Worker>();
            services.AddHostedService<LiveStatsWorker>();

            // Add CORS policy
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000") // Specify your frontend origin here
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });

            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(ConnectionString.GetConnectionString()));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Use CORS policy
            app.UseCors("AllowSpecificOrigin");

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                string url = "/stats-hub";
                endpoints.MapHub<StatHub>(url);
            });
        }
    }
}