using Antopia.Domain.Entities.MensajeE;
using Microsoft.AspNetCore.SignalR;

namespace Antopia.API.Chat
{
    public class CentroChat : Hub
    {

        private static readonly Dictionary<string, string> SalasPrivadas = new Dictionary<string, string>();

        // Esta lista almacenará mensajes por sala de chat privada
        private static readonly Dictionary<string, List<MensajeE>> MensajesPorSala = new Dictionary<string, List<MensajeE>>();

        public async Task EnviarMensaje(string mensaje, string destinatarioId)
        {
            var remitenteId = Context.UserIdentifier;
            var salaId = GenerarSalaId(remitenteId, destinatarioId);

            await Groups.AddToGroupAsync(Context.ConnectionId, salaId);

            var mensajeE = new MensajeE
            {
                UsuarioId = int.Parse(remitenteId), // Convierte a entero el ID del usuario actual
                s_contenido = mensaje,
                dt_fechaEnvio = DateTime.Now
            };

            // Almacena el mensaje en la lista de mensajes para esta sala
            if (!MensajesPorSala.ContainsKey(salaId))
            {
                MensajesPorSala[salaId] = new List<MensajeE>();
            }
            MensajesPorSala[salaId].Add(mensajeE);

            // Enviar el mensaje solo al destinatario en la sala privada
            await Clients.Group(salaId).SendAsync("RecibirMensaje", mensajeE);
        }

        // Otros métodos del ChatHub según sea necesario

        private string GenerarSalaId(string userId1, string userId2)
        {
            var usuariosOrdenados = string.Compare(userId1, userId2) < 0
                ? $"{userId1}_{userId2}"
                : $"{userId2}_{userId1}";

            return $"Sala_{usuariosOrdenados}";
        }

    }
}
