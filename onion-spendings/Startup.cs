using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Spendings.Orchrestrators.Users;
using Spendings.Orchrestrators.Users.Contracts;
using Spendings.Core.Users;
using Spendings.Data.DB;
using Spendings.Data.Users;
using Spendings.Core.Records;
using Spendings.Data.Records;
using Spendings.Data.Categories;
using Spendings.Orchrestrators.Records;
using Spendings.Core.Categories;
using Spendings.Orchrestrators.Categories;
using Spendings.Orchrestrators.Categories.Contracts;

namespace onion_spendings
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option =>
            {
                option.EnableEndpointRouting = false;
                option.Filters.Add(typeof(ExceptionFilter));
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Spendings Api",
                    Description = ""
                });
            });
            services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.ToString());
            });
            services.AddControllers();
            services.AddAutoMapper(typeof(OrchUserProfile),typeof(UserDaoProfile),
                typeof(RecordDaoProfile),typeof(CategoryDaoProfile),typeof(CategoryProfile));

            string connString = Configuration.GetConnectionString("SpendingsDB");
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connString));

            services.AddScoped<IUserRepository, UsersRepository>();
            services.AddScoped<IUserService, UsersService>();
            services.AddScoped<IRecordRepository, RecordsRepository>();
            services.AddScoped<IRecordService, RecordsService>();
            services.AddScoped<ICategoryService, CategoriesService>();
            services.AddScoped<ICategoryRepository, CategoriesRepository>();
            services.AddCors();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(builder => builder.
            AllowAnyOrigin().
            AllowAnyMethod().
            AllowAnyHeader());
            
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Spendings API V1");
            });

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
