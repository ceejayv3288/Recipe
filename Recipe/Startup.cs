using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Recipe.Data;
using Recipe.Mappers.RecipeMapper;
using Recipe.Repositories;
using Recipe.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Recipe
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
            services.AddDbContext<ApplicationDbContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IRecipeRepository, RecipeRepository>();
            services.AddScoped<IRecipeStepRepository, RecipeStepRepository>();
            services.AddScoped<IRecipeIngredientRepository, RecipeIngredientRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ILikeRepository, LikeRepository>();
            services.AddAutoMapper(typeof(RecipeMappings));
            services.AddControllers();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("RecipeOpenAPISpec",
                                    new OpenApiInfo()
                                    {
                                        Title = "Recipe API",
                                        Version = "v1",
                                        Description = "Recipe API",
                                        Contact = new OpenApiContact()
                                        {
                                            Email = "ceejayv328@gmail.com",
                                            Name = "Christian Joseph Vargas",
                                            Url = new Uri("https://www.linkedin.com/in/christian-joseph-vargas-0001481a3/")
                                        },
                                        License = new OpenApiLicense()
                                        {
                                            Name = "MIT License",
                                            Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
                                        }
                                    });
                var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
                options.IncludeXmlComments(xmlCommentsFullPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/RecipeOpenAPISpec/swagger.json", "Recipe API");
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
