using System;
using System.IO;
using System.Threading.Tasks;

namespace Antopia.Persistence.ImageService
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(string base64Image, string ruta);
        Task<bool> DeleteImageAsync(string imagePath, int tipo);
        Task<bool> DeleteImage(string ruta);
    }

    public class ImageService : IImageService
    {

        private string rutaUrl = "http://localhost:5239/";
        //private string rutaUrl = "https://antopia.site/";

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
                string[] rutaDos = ruta.Split('/');
                string rutaImagen = rutaUrl + rutaDos[1] + "/"  + fileName;

                return rutaImagen;
            }
            catch (Exception ex)
            {
                throw ex;
         
            }
        }

        public async Task<bool> DeleteImageAsync(string imagePath, int tipo)
        {
            try
            {
                string[] urlParts = imagePath.Split('/');
                string fileName = urlParts[urlParts.Length - 1];
                string fullPath = "";
                if (tipo == 2)
                {
                    fullPath = Path.Combine("wwwroot/", "ImagesPerfil", fileName);
                } else
                {
                    fullPath = Path.Combine("wwwroot/", "ImageFondo", fileName);
                }
                

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> DeleteImage(string imageUrl)
        {
            try
            {
               
                var localPath = imageUrl.Replace(rutaUrl, "");

                // Combinar con la ruta completa de "wwwroot"
                var fullPath = Path.Combine("wwwroot", localPath);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




    }


}
