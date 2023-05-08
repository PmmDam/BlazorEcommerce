namespace BlazorEcommerce.Server.Services.AuthService
{
    public interface IAuthService
    {
        /// <summary>
        /// Creamos un usuario en la base de datos. Primero comprobamos si existe y en caso contrario, hasheamos la contraseña para guardarla en la base de datos
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<ServiceResponse<int>> Register(UserModel user,string password);
        Task<bool> UserExists(string email);
    }
}
