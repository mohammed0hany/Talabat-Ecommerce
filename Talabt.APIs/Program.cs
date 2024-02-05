using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Security.Cryptography.Xml;
using Talabat.Core;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;
using Talabat.Service;
using Talabt.APIs.Errors;
using Talabt.APIs.Helpers.MappingProfiles;
using Talabt.APIs.Middlewares;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Talabt.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region ConfigureSevices (Allow dependency injection for services)
            webApplicationBuilder.Services.AddControllers();



            webApplicationBuilder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
            }); 
            
            webApplicationBuilder.Services.AddDbContext<AppIdentityDbContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("IdentityConnection"));
            });

            webApplicationBuilder.Services.AddSingleton<IConnectionMultiplexer>(serviceprovide =>
            {
                var connection = webApplicationBuilder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            });
            webApplicationBuilder.Services.AddScoped(typeof(IOrderService), typeof(OrderService));
            webApplicationBuilder.Services.AddScoped(typeof(IProductService), typeof(ProductService));
            webApplicationBuilder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            webApplicationBuilder.Services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
            //webApplicationBuilder.Services.AddScoped(typeof(IGenaricRepository<>), typeof(GenaricRepository<>));
            webApplicationBuilder.Services.AddScoped<IBasketRepository, BasketRepository>();
            //webApplicationBuilder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfile()));

            webApplicationBuilder.Services.AddAutoMapper(typeof(MappingProfile));
            webApplicationBuilder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();
            webApplicationBuilder.Services.AddScoped(typeof(IAuthService),typeof(AuthService));
            webApplicationBuilder.Services.AddSingleton(typeof(IResponseCacheService),typeof(ResponseCacheService));
            webApplicationBuilder.Services.AddAuthentication(options=> 
                                           {
                                               options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                                               options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                                            })
                                          .AddJwtBearer(options =>
                                                        options.TokenValidationParameters = new TokenValidationParameters()
                                                        {
                                                            ValidateAudience = true,
                                                            ValidAudience = webApplicationBuilder.Configuration["JWT:ValidAudience"],
                                                            ValidateIssuer = true,
                                                            ValidIssuer = webApplicationBuilder.Configuration["JWT:ValidIssuer"],
                                                            ValidateIssuerSigningKey = true,
                                                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(webApplicationBuilder.Configuration["JWT:SecretKey"])),
                                                            ValidateLifetime = true,
                                                            ClockSkew = TimeSpan.FromDays(double.Parse(webApplicationBuilder.Configuration["JWT:DurationInDays"]))
                                                        }
                );
            webApplicationBuilder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", options =>
                {
                    options.AllowAnyHeader().AllowAnyMethod().WithOrigins(webApplicationBuilder.Configuration["FrontBaseUrl"]);
                });
            });


            webApplicationBuilder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count()>0)
                                                         .SelectMany(P=>P.Value.Errors)
                                                         .Select(E=>E.ErrorMessage)
                                                         .ToArray();
                    var validationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(validationErrorResponse);
                };
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            webApplicationBuilder.Services.AddEndpointsApiExplorer();
            webApplicationBuilder.Services.AddSwaggerGen();


            var app = webApplicationBuilder.Build();
            using var scope= app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var _dbcontext = services.GetRequiredService<StoreContext>(); // batlob mn el clr object explicitly
            var _Identitydbcontext = services.GetRequiredService<AppIdentityDbContext>();// batlob mn el clr object explicitly
            var loggerfactory = services.GetRequiredService<ILoggerFactory>();


            try
            {
                await _dbcontext.Database.MigrateAsync();
                await StoreContextSeed.SeedAsync(_dbcontext);
                await _Identitydbcontext.Database.MigrateAsync();
                var _userManager = services.GetRequiredService<UserManager<AppUser>>(); 
                await AppIdentityDbContextSeed.SeedUsersAsync(_userManager);


            }
            catch (Exception ex)
            {
                var logger = loggerfactory.CreateLogger<Program>();
                logger.LogError(ex, "an error have been accures while aplling migration");
                
            }


            #endregion



            #region Configure (Kestrel Middlewares)
            app.UseMiddleware<ExceptionMiddleware>(); 


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("MyPolicy");
            //app.UseAuthorization();

            //badal el userRouting w useEndPoints f el MVC
            app.MapControllers();
            app.UseAuthentication();
            app.UseAuthorization();
            #endregion

            app.Run();
        }
    }
}