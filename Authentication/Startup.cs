using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
//using FrameWork.Authentication.Infrastructure.Repositories;
using FrameWork.Authentication.Domain.Models;
using Microsoft.EntityFrameworkCore;
//using FrameWork.Authentication.Domain.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Framework.Authentication.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Framework.Authentication;
using Microsoft.OpenApi.Models;
using System;
using Framework.Authentication.Infrastructure.Repositories;

namespace FrameWork.Authentication
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
            services.AddControllersWithViews();
            // add this line to make sure that controllers can 
            // supress the naming convention policy
            services.AddControllers().AddNewtonsoftJson(options => {
                options.UseCamelCasing(false);
                options.UseMemberCasing();
            });
            
            services.AddDbContext<AuthorizationDBContext>(options =>
            {
                options.UseCosmos(Configuration["AppSettings:EndPoint"].ToString(),
                  Configuration["AppSettings:Key"].ToString(),
                   Configuration["AppSettings:DatabaseId"].ToString());
            });

            services.AddSwaggerGen(setup =>
            {

                // Include 'SecurityScheme' to use JWT Authentication
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                setup.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    { jwtSecurityScheme, Array.Empty<string>() }});
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // Adding Jwt Bearer
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });

            services.Configure<ConfigurationManager>(Configuration.GetSection("ConfigurationManager"));
            services.AddScoped<IAuthenticationQuery, AuthenticationQuery>();

            // configure strongly typed settings object
            // services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            
            services.AddSwaggerGen();
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
                app.UseExceptionHandler("/Error");
            }

            app.UseCors(x => x
              .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());

            app.UseHttpsRedirection();

            // app.UseStaticFiles();
            //if (!env.IsDevelopment())
            //{
            //    app.UseSpaStaticFiles();
            //}

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // custom jwt auth middleware
            //app.UseMiddleware<JwtMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Authentication API");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Authentication}");
            });
        }
    }
}
