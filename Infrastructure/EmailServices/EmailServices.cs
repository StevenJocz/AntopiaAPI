using Antopia.Domain.DTOs.EmailDTOs;
using MailKit.Security;
using Microsoft.Extensions.Configuration;

using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace Antopia.Infrastructure.EmailServices
{
    public interface IEmailServices
    {
        Task<EmailResponse> EmailRestablecimientoPassword(EmailDTOs request);
        Task<bool> EmailCreateUser(string correo);
        Task<bool> InvitarDiario(int idPerfil, string NombreUser);
    }

    public class EmailServices : IEmailServices
    {
        private readonly IConfiguration _configuration;
        private readonly AntopiaDbContext _context = null;

        public EmailServices(IConfiguration configuration)
        {
            _configuration = configuration;
            string? connectionString = _configuration.GetConnectionString("Connection");
            _context = new AntopiaDbContext(connectionString);
        }

        public async Task<bool> EnviarEmail(int Accion, EmailDTOs request, string nombre, string codigo)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_configuration.GetSection("Email:UserName").Value));
                email.To.Add(MailboxAddress.Parse(request.Para));
                email.Subject = request.Asunto;
                string contenido = request.Contenido.Replace("[Nombre]", nombre);

                if (Accion == 1)
                {
                    contenido = contenido.Replace("[Código de seguridad]", codigo);
                }

                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = contenido
                };

                using var smtp = new SmtpClient();
                smtp.Connect(
                    _configuration.GetSection("Email:Host").Value,
                    Convert.ToInt32(_configuration.GetSection("Email:Port").Value),
                    SecureSocketOptions.StartTls
                );

                smtp.Authenticate(_configuration.GetSection("Email:UserName").Value, _configuration.GetSection("Email:PassWord").Value);

                smtp.Send(email);
                smtp.Disconnect(true);

                return true;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public string GenerarCodigo(int longitud)
        {
            const string caracteres = "0123456789";
            var random = new Random();

            var codigoBuilder = new StringBuilder(longitud);
            for (int i = 0; i < longitud; i++)
            {
                codigoBuilder.Append(caracteres[random.Next(caracteres.Length)]);
            }
            return codigoBuilder.ToString();
        }

        public async Task<EmailResponse> EmailRestablecimientoPassword(EmailDTOs request)
        {
            var Email = _context.UserEs.FirstOrDefault(x => x.s_user_email == request.Para);
            if (Email == null)
            {
                return new EmailResponse
                {
                    resultado = false,
                    message = "No se encontró ningún usuario asociado al correo electrónico proporcionado. Por favor, verifica la dirección de correo electrónico ingresada  ",
                    codigo = null
                };
            }
            else
            {
                int Accion = 1;
                string codigo = GenerarCodigo(6);
                string NombreCompleto = Email.s_user_name;
                bool Enviado = await EnviarEmail(Accion, request, NombreCompleto, codigo);

                if (Enviado)
                {

                    return new EmailResponse
                    {
                        resultado = true,
                        message = "Se ha enviado un correo electrónico correctamente.",
                        codigo = codigo
                    };
                }
                else
                {
                    return new EmailResponse
                    {
                        resultado = false,
                        message = "Tenemos problemas al enviar el correo electrónico. Por favor intentalo más tarde.",
                        codigo = null
                    };
                }

            }
        }

        public async Task<bool> EmailCreateUser(string correo)
        {

            var request = new EmailDTOs
            {
                Para = correo,
                Asunto = "Bienvenidos a Antopia",
                Contenido = @"<!DOCTYPE html>
                                <html>
                                <head>
                                    <meta charset=""UTF-8"">
                                    <title>Bienvenidos a Antopia</title>
                                    <style>
                                        body {
                                            font-family: Arial, sans-serif;
                                            background-color: #f4f4f4;
                                            color: #333;
                                        }
                                        .container {
                                            max-width: 600px;
                                            margin: 0 auto;
                                            padding: 20px;
                                        }
                                        h1 {
                                            color: #555;
                                        }
                                        p {
                                            margin-bottom: 10px;
                                        }
                                        .footer {
                                            margin-top: 30px;
                                            font-size: 14px;
                                            color: #777;
                                        }
                                    </style>
                                </head>
                                <body>
                                    <div class=""container"">
                                        <h1>Bienvenido a Antopia</h1>
                                        <p>Hola [Nombre],</p>
                                        <p>Te damos la más cordial bienvenida a Antopia, la comunidad dedicada a la mirmecología y a todos los apasionados por las hormigas.</p>
                                        <p>Estamos emocionados de que te hayas unido a nosotros. En Antopia, encontrarás un espacio donde puedes compartir tu amor por las hormigas, aprender más sobre ellas y conectarte con personas que comparten tu misma pasión.</p>
                                        <p class=""footer"">Atentamente,<br>El equipo de Antopia</p>
                                    </div>
                                </body>
                                </html>"
                };

            var Email = _context.UserEs.FirstOrDefault(x => x.s_user_email == correo);
            string NombreCompleto = Email.s_user_name;
            bool Enviado = await EnviarEmail(2, request, NombreCompleto, "");

            if (Enviado)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public async Task<bool> InvitarDiario(int idPerfil, string NombreUser)
        {
            var Email = _context.UserEs.FirstOrDefault(x => x.id == idPerfil);
            string Para = Email.s_user_email;

            var request = new EmailDTOs
            {
                Para = Para,
                Asunto = "¡Invitación a crear tu primer diario en Antopia!",
                Contenido = @"<!DOCTYPE html>
                        <html>
                        <head>
                            <meta charset=""UTF-8"">
                            <title>¡Bienvenido al Diario de Colonias de Antopia!</title>
                            <style>
                                body {
                                    font-family: Arial, sans-serif;
                                    background-color: #f4f4f4;
                                    color: #333;
                                }

                                .container {
                                    max-width: 600px;
                                    margin: 0 auto;
                                    padding: 20px;
                                }

                                h1 {
                                    color: #555;
                                }

                                p {
                                    margin-bottom: 10px;
                                }

                                .code {
                                    background-color: #f9f9f9;
                                    border: 1px solid #ddd;
                                    padding: 10px;
                                    font-size: 20px;
                                }

                                .footer {
                                    margin-top: 30px;
                                    font-size: 14px;
                                    color: #777;
                                }
                            </style>
                        </head>

                        <body>
                            <div class=""container"">
                                <h1>Bienvenido al Diario de Colonias de Antopia</h1>
                                <p>[Nombre] te ha invitado a empezar tu viaje diario con nosotros en el fascinante mundo de las colonias de hormigas en Antopia.</p>
                            </div>
                        </body>

                        </html>
                        "
            };

            bool Enviado = await EnviarEmail(2, request, NombreUser, "");

            if (Enviado)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
