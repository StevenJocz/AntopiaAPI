using System;
using System.IO;
using System.Threading.Tasks;

namespace Antopia.Persistence.ImageService
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(string base64Image, string ruta);
    }

    public class ImageService : IImageService
    {
        public async Task<string> SaveImageAsync(string base64Image,string ruta)
        {
            try
            {
                string[] base64Parts = base64Image.Split(',');

                if (base64Parts.Length != 2)
                {
                    throw new ArgumentException("La cadena Base64 no tiene el formato esperado.");
                }

                // La segunda parte contiene la representación Base64 de la imagen
                string base64Data = base64Parts[1];
                byte[] imageBytes = Convert.FromBase64String(base64Data);
                string fileName = $"{Guid.NewGuid()}.jpg";
                string filePath = Path.Combine(ruta, fileName); 
                await File.WriteAllBytesAsync(filePath, Convert.FromBase64String(base64Data));

                string rutaImagen = "http://localhost:5239/Images/" + fileName;

                return rutaImagen;
            }
            catch (Exception ex)
            {
                throw ex;
         
            }
        }

       
    }

   
}
