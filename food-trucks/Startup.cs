using food_trucks.Services;
using food_trucks.Services.Dummy;
using food_trucks.Services.Implemntation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace food_trucks
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
            var fileName = Configuration["csvFilePath"];
            Console.WriteLine($" CSV File path is {fileName}");
            // Add the GeoDecoder
            services.AddSingleton<IGeoDecoder>(x => new BasicGeoDecoder(x.GetRequiredService<ILogger<BasicGeoDecoder>>()));
            // Adds the CSV inatilzer
            services.AddSingleton<IFoodTruckInitailizer>(x => new CSVTruckInatilizer(x.GetRequiredService<ILogger<CSVTruckInatilizer>>(), fileName));

            // Add the Lucene Full search text database
            services.AddSingleton<IFullTextSearchFoodTruck>(x => new LuceneInMemoryFullTextSearch(x.GetRequiredService<ILogger<LuceneInMemoryFullTextSearch>>()));

            // Adds The main Database Context, the main backing store
            services.AddSingleton<IFoodTruckContext>
                (x => new MemoryBacked(x.GetRequiredService<ILogger<MemoryBacked>>(), x.GetRequiredService<IFoodTruckInitailizer>(), x.GetRequiredService<IFullTextSearchFoodTruck>()));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "food_trucks", Version = "v1" });
            });
           

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "food_trucks v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
