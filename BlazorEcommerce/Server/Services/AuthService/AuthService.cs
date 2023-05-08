using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Security.Cryptography;

namespace BlazorEcommerce.Server.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<string>> LoginAsync(string email, string password)
        {
            var response = new ServiceResponse<string>();
            var user  = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));

            if(user == null)
            {
                response.Success = false;
                response.Message = "Usuario no encontrado.";
            }

            else if(!VerifyPasswordHash(password,user.PasswordHash,user.PasswordSalt)) 
            {
                response.Success = false;
                response.Message = "Contraseña incorrecta.";
            }
            else
            {
                response.Data = CreateToken(user);
            }
            
            
            return response;
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
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(UserModel user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),

            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims:claims,
                expires:DateTime.Now.AddDays(1),
                signingCredentials:creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

    }
}
