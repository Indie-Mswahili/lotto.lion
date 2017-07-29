using LottoLion.BaseLib.Models.Entity;
using LottoLion.BaseLib.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OdinSdk.BaseLib.Configuration;
using OdinSdk.BaseLib.Cryption;
using System;
using System.Text;

namespace LottoLion.WebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEncryptedProvider()
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        private const string __security_key = "13E762DEBE39D65E57F4FD4C55F7D";
        private readonly SymmetricSecurityKey __signing_key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(__security_key));

        private static string __connection_string = null;
        private string ConnectionString
        {
            get
            {
                if (__connection_string == null)
                {
                    var _key = Configuration["aes_key"];
                    var _iv = Configuration["aes_iv"];

                    var _cryptor = new CCryption(_key, _iv);

                    __connection_string = _cryptor.ChiperToPlain(Configuration.GetConnectionString("DefaultConnection"));
                }

                return __connection_string;
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);

            services.AddDbContext<LottoLionContext>(builder =>
            {
                builder.UseNpgsql(ConnectionString);
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            // Add framework services.
            services.AddOptions();

            // Make authentication compulsory across the board (i.e. shutdown EVERYTHING unless explicitly opened up).
            {
                services.AddMvc(config =>
                {
                    var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();

                    config.Filters.Add(new AuthorizeFilter(policy));
                });

                // Use policy auth.
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("LottoLionGuest",
                                policy =>
                                {
                                    policy.RequireClaim("UserType", "Guest");
                                });

                    options.AddPolicy("LottoLionMember",
                                policy =>
                                {
                                    policy.RequireClaim("UserType", "Member");
                                });

                    options.AddPolicy("LottoLionUsers",
                                policy =>
                                {
                                    policy.RequireRole("ValidUsers");
                                });
                });

                // Get options from app settings
                var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

                // Configure JwtIssuerOptions
                services.Configure<JwtIssuerOptions>(options =>
                {
                    options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                    options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                    options.ValidFor = TimeSpan.FromMinutes(Convert.ToInt32(jwtAppSettingOptions[nameof(JwtIssuerOptions.ValidFor)]));
                    options.SigningCredentials = new SigningCredentials(__signing_key, SecurityAlgorithms.HmacSha256);
                });
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = __signing_key,

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters
            });

            // global policy - assign here or on each controller
            app.UseCors("CorsPolicy");

            app.UseMvc();
        }
    }
}
