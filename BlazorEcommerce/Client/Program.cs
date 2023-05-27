//Proyecto con los modelos
global using BlazorEcommerce.Shared;

//Servicios
global using BlazorEcommerce.Client.Services.CategoryService;
global using BlazorEcommerce.Client.Services.ProductService;
global using BlazorEcommerce.Client.Services.CartService;
global using BlazorEcommerce.Client.Services.AuthService;
global using BlazorEcommerce.Client.Services.OrderService;
global using BlazorEcommerce.Client.Services.AddressService;

//Paquetes Nuget
global using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;

namespace BlazorEcommerce.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            //registramos las implementaciones concretas a las interfaces correspondientes en el contenedor de dependencias
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IOrderService,OrderService>();
            builder.Services.AddScoped<IAddressService, AddressService>();


            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

            await builder.Build().RunAsync();
        }
    }
}