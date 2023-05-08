using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace BlazorEcommerce.Server.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;

        public AuthService(DataContext context)
        {
            _context = context;
        }

       
        public async Task<ServiceResponse<int>> Register(UserModel user, string password)
        {
            //Clausula guarda
            if (await UserExists(user.Email))
            {
                return new ServiceResponse<int> { Success = false, Message = "El email está en uso" };
            }

            //Hasheamos la contraseña
            CreatePasswordHash(password,out byte[] passwordHash,out byte[] salt); 
            
            //asignamos el salt y el hash al usuario
            user.PasswordSalt = salt;
            user.PasswordHash = passwordHash;

            //Lo añadimos al contexto/sesion de la base de datos
            _context.Users.Add(user);

            //Actualizamos la base de datos
            await _context.SaveChangesAsync();

            //Devolvemos la respuesta con el id del usuario
            return new ServiceResponse<int> { Data = user.Id,Message="Registro completado!" };
        }
        public async Task<bool> UserExists(string email)
        {
            bool result = false;
            if(await _context.Users.AnyAsync(x => x.Email.ToLower().Equals(email.ToLower())))
            {
                result = true;
            }
            return result;
        }

       private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
