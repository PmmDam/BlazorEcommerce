namespace BlazorEcommerce.Client.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> RegisterAsync(UserRegister request);
        Task<ServiceResponse<string>> Login(UserLogin request);
    }
}
