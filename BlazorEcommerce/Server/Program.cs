global using BlazorEcommerce.Shared;
global using Microsoft.EntityFrameworkCore;
global using BlazorEcommerce.Server.Data;
global using BlazorEcommerce.Server.Services.ProductService;
global using BlazorEcommerce.Server.Services.CategoryService;
global using BlazorEcommerce.Server.Services.AuthService;

using Microsoft.AspNetCore.ResponseCompression;
using BlazorEcommerce.Server.Services.CartService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace BlazorEcommerce
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //Añadimos el contecto de nuestra base de datos y nos conectamos con la cadena de conexión por defecto almacenada en appsettings.json
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                //options.UseSqlServer(builder.Configuration.GetConnectionString("ProductionConnection"));
            });

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            //Añadimos las dependencias de Swagger para poder añadir una interfaz de usuario a nuestras APIs
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Inyectamos las dependencias de las clases creadas en el proyecto
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            
            //Con este servicio podremos extraer y validar el JSON Web Token para
            //que el usuario pueda realizar operaciones como el cambio de contraseña unicamente si está autenticado y autorizado
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            //Le decimos a la aplicación que utilice la UI de Swagger
            app.UseSwaggerUI();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //Le decimos a la aplicación que utilice Swagger
            app.UseSwagger();


            app.UseHttpsRedirection();

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();
            app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}