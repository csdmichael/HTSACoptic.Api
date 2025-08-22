using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using HTSA.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using HTSA.Models;
using HTSA.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using TokenAuth.Repositories;
using TokenAuth.Models;

namespace HTSA
{
    public class Startup
    {
        const string TokenAudience = "htsa.org";
        const string TokenIssuer = "HTSA";
        private RsaSecurityKey key;
        private TokenAuthOptions tokenOptions;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Begin configure services for TokenAuth
            // Read RSA Key Params from secret file
            string file = "RSAParams";
            RSAParameters keyParams = RSAKeyUtils.GetKeyParameters(file);

            // Create the key, and a set of token options to record signing credentials 
            // using that key
            key = new RsaSecurityKey(keyParams);
            tokenOptions = new TokenAuthOptions()
            {
                Audience = TokenAudience,
                Issuer = TokenIssuer,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature)
            };

            // Save the token options into an instance so they're accessible to the 
            // controller.
            services.AddSingleton<TokenAuthOptions>(tokenOptions);

            // Enable the use of an [Authorize("Bearer")] attribute on methods and classes to protect.
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
                auth.DefaultPolicy = auth.GetPolicy("Bearer");
            });

            services.AddAuthentication().AddJwtBearer(o =>
            {
                o.Audience = tokenOptions.Audience;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = key,
                    ValidAudience = tokenOptions.Audience,
                    ValidIssuer = tokenOptions.Issuer,
                    // When receiving a token, check that it is still valid.
                    ValidateLifetime = true,
                    // This defines the maximum allowable clock skew
                    ClockSkew = System.TimeSpan.FromMinutes(1)
                };
            });

            services.Add(new ServiceDescriptor(typeof(TokenAuthOptions), tokenOptions));
            services.AddTransient<IAuthRepository, AuthRepository>();


            services.AddTransient<IEventLogInfoRepository, EventLogInfoRepository>();
            
            services.AddTransient<ILoginsInfoRepository, LoginsInfoRepository>();

            // services.AddTransient<ISSClassRepository, SSClassRepository>();

            // services.AddTransient<IKidRepository, KidRepository>();

            services.AddTransient<IPersonRepository, PersonRepository>();

            // services.AddTransient<ILessonRepository, LessonRepository>();

            services.AddTransient<IPostRepository, PostRepository>();
            services.AddTransient<IAppRepository, AppRepository>();

            // services.AddTransient<ICarRepository, CarRepository>();

            services.AddTransient<IChurchRepository, ChurchRepository>();

            // services.AddTransient<IPrivacyRepository, PrivacyRepository>();

            services.AddTransient<IBibleRepository, BibleRepository>();
            services.AddTransient<IBibleFavsRepository, BibleFavsRepository>();
            services.AddTransient<ICalendarRepository, CalendarRepository>();
            services.AddTransient<INotifRepository, NotifRepository>();
            services.AddTransient<IPushRepository, PushRepository>();
            services.AddTransient<IFileRepository, FileRepository>();
            //--End-----------------------------------------------

            services.Add(new ServiceDescriptor(typeof(IConfigurationRoot), Configuration));
            services.Configure<IISOptions>(options =>
            {
                options.AutomaticAuthentication = true;
            });
            services.AddLogging();
            services.AddRouting();
            services.AddMvc();
            services.AddCors();

            services.AddDbContext<TokenAuth.Data.AuthContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("CPConnection")));

            services.AddDbContext<ApplContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("CPConnection")));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging")).AddDebug().AddFile("logs/lsapi-{Date}.txt");
            loggerFactory.AddDebug();

            // Register a simple error handler to catch token expiries and change them to a 401, 
            // and return all other errors as a 500. This should almost certainly be improved for
            // a real application.
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Use(async (context, next) =>
                {
                    var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;
                    // This should be much more intelligent - at the moment only expired 
                    // security tokens are caught - might be worth checking other possible 
                    // exceptions such as an invalid signature.
                    if (error != null && error.Error is SecurityTokenExpiredException)
                    {
                        context.Response.StatusCode = 401;
                        // What you choose to return here is up to you, in this case a simple 
                        // bit of JSON to say you're no longer authenticated.
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(
                            JsonConvert.SerializeObject(
                                new { authenticated = false, tokenExpired = true }));
                    }
                    else if (error != null && error.Error != null)
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";
                        // TODO: Shouldn't pass the exception message straight out, change this.
                        await context.Response.WriteAsync(
                            JsonConvert.SerializeObject
                            (new { success = false, error = error.Error.Message }));
                    }
                    // We're not trying to handle anything else so just let the default 
                    // handler handle.
                    else await next();
                });
            });

                
            /*
            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audience,
                    IssuerSigningKey = key,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ClockSkew = TimeSpan.FromMinutes(0)
                }
            });
            */

            app.UseCors(builder => builder
                //.WithOrigins("http://*")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowCredentials());

            app.UseMvcWithDefaultRoute();

            app.UseStaticFiles();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                Formatting = Formatting.Indented
            };
        }
    }
}
