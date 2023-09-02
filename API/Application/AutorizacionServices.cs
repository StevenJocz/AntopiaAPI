using Antopia.Domain.DTOs.LoginDTOs;
using Antopia.Domain.Entities.LoginE;
using Antopia.Infrastructure;
using Antopia.Persistence.Queries.LoginQueries;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Antopia.API.Application
{

    public interface IAutorizacionService
    {
        Task<AutorizacionResponse> DevolverToken(LoginDTOs autorizacion);
    }

    public class AutorizacionService : IAutorizacionService
    {
        private readonly AntopiaDbContext _context = null;
        private readonly ILogger<AutorizacionService> _logger;
        private readonly IConfiguration _configuration;
        private readonly ILoginQueries _loginQueries;

        public AutorizacionService(ILogger<AutorizacionService> logger, IConfiguration configuration, ILoginQueries loginQueries)
        {
            _logger = logger;
            _configuration = configuration;
            string? connectionString = _configuration.GetConnectionString("Connection_Salud");
            _context = new AntopiaDbContext(connectionString);
            _loginQueries = loginQueries;
        }

        private async Task<string> GenerarToken(string IdUsuario)
        {
            try
            {
                _logger.LogInformation("Iniciando GenerarToken");
                var key = _configuration.GetValue<string>("JwtSettings:key");
                var keyBytes = Encoding.ASCII.GetBytes(key);

                var usuario = await _loginQueries.ConsultarUsuarioXId(int.Parse(IdUsuario));

                var claims = new ClaimsIdentity();
                claims.AddClaim(new Claim("IdPerfil", Convert.ToString(usuario.id)));
                claims.AddClaim(new Claim("NombrePerfil", usuario.s_user_name));
                claims.AddClaim(new Claim("ImagenPerfil", usuario.s_userPhoto));
                claims.AddClaim(new Claim("urlPerfil", usuario.s_userProfile));
                claims.AddClaim(new Claim("rol", Convert.ToString(usuario.fk_tblRol)));
                claims.AddClaim(new Claim("email", usuario.s_user_email));

                var credencialesToken = new SigningCredentials
                (
                   new SymmetricSecurityKey(keyBytes),
                   SecurityAlgorithms.HmacSha256Signature
                );

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddMinutes(1),
                    SigningCredentials = credencialesToken
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

                string tokenCreado = tokenHandler.WriteToken(tokenConfig);

                return tokenCreado;
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar GenerarToken");
                throw;
            }
        }


        private async Task<AutorizacionResponse> GuardarHistorialRefreshToken(int IdUsuario, string Token)
        {
            try
            {
                _logger.LogInformation("Iniciando GuardarHistorialRefreshToken");
                var historialRefreshToken = new HistorialrefreshtokenE
                {
                    idhistorialtoken = 0,
                    idusuario = IdUsuario,
                    s_token = Token,
                    ts_fechacreacion = DateTime.UtcNow,
                    ts_fechaexpiracion = DateTime.UtcNow.AddMinutes(2)
                };

                await _context.HistorialrefreshtokenEs.AddAsync(historialRefreshToken);
                await _context.SaveChangesAsync();

                return new AutorizacionResponse
                {
                    Token = Token,
                    Resultado = true,
                    Msg = "Ok"
                };
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar GuardarHistorialRefreshToken");
                throw;
            }
        }


        public async Task<AutorizacionResponse> DevolverToken(LoginDTOs autorizacion)
        {
            try
            {
                _logger.LogInformation("Iniciando DevolverToken");

                var usuario_Encontrado = await _loginQueries.ConsultarUsuarioXCorreo(autorizacion.userEmail, autorizacion.userPassword);

                if (usuario_Encontrado == null)
                {
                    return new AutorizacionResponse()
                    {
                        Resultado = false,
                        Msg = "Las credenciales de correo electrónico o contraseña proporcionadas son inválidas. Por favor, verifica la información ingresada e intenta nuevamente."
                    };
                }

                string tokenCreado = await GenerarToken(usuario_Encontrado.IdLogin.ToString());
                await GuardarHistorialRefreshToken(int.Parse(usuario_Encontrado.IdLogin.ToString()), tokenCreado);

                return new AutorizacionResponse()
                {
                    Token = tokenCreado,
                    Resultado = true,
                    Msg = "Ok"
                };
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar DevolverToken");
                throw;
            }
        }
    }
}
