using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration; // Para leer la cadena de conexión
using System;
using System.IO; // Necesario para Stream
using System.Threading.Tasks; // Necesario para Task

namespace LoftComputacion.Application
{
    public class BlobService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(IConfiguration configuration)
        {
            // Lee la cadena de conexión que guardamos en appsettings.json
            var connectionString = configuration.GetConnectionString("BlobStorageConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("La cadena de conexión de Blob Storage no está configurada.");
            }
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        /// <summary>
        /// Sube un archivo a Azure Blob Storage.
        /// </summary>
        /// <param name="fileStream">El contenido (bytes) del archivo a subir.</param>
        /// <param name="fileName">El nombre que tendrá el archivo en la nube (ej: "foto-orden-5.jpg").</param>
        /// <param name="containerName">El nombre del "contenedor" o "carpeta" en Azure (ej: "fotos").</param>
        /// <returns>La URL pública del archivo subido.</returns>
        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string containerName)
        {
            // 1. Obtiene o crea el contenedor (carpeta)
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob); // Lo hacemos público para que se puedan ver las fotos

            // 2. Obtiene una referencia al "blob" (el archivo)
            var blobClient = containerClient.GetBlobClient(fileName);

            // 3. Sube el contenido (el stream de la imagen comprimida)
            await blobClient.UploadAsync(fileStream, overwrite: true);

            // 4. Devuelve la URL pública del archivo
            return blobClient.Uri.ToString();
        }

        public async Task DeleteFileAsync(string fileName, string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.DeleteIfExistsAsync();
        }
    }
}