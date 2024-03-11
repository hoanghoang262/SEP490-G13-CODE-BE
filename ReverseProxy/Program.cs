using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using System.Text;

namespace ReverseProxy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            //builder.Services.AddReverseProxy()

            //    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxyAccount"))
            //    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxyAuthen"));
            //builder.Services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("customPolicy", policy =>
            //        policy.RequireAuthenticatedUser());
            //});

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
                };
            });

            builder.Services.AddSwaggerForOcelot(builder.Configuration, x =>
            {
                x.GenerateDocsForGatewayItSelf = false;

            });
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("ocelot.json").Build();
            builder.Configuration.AddJsonFile("ocelot.json").Build();
            builder.Services.AddOcelot(configuration).AddPolly();

            var app = builder.Build();
            app.UseHttpsRedirection();


            app.UseHttpsRedirection();
          
            app.UseAuthorization();


           
            app.MapControllers();
            app.UseAuthentication();

            app.UseSwaggerForOcelotUI(opt =>
            {
                opt.PathToSwaggerGenerator = "/swagger/docs";
            });
            app.UseOcelot();

            // app.MapReverseProxy();

            app.Run();
        }
    }
}