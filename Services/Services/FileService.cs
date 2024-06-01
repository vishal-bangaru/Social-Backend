using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Services.Services
{
    public class FileService : FileServiceInterface
    {
        private readonly BlobServiceClient blobServiceClient;
        public FileService(BlobServiceClient _blobServiceClient)
        {
            this.blobServiceClient = _blobServiceClient;
        }

        public async Task<string> Upload(IFormFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            var containerInstance = blobServiceClient.GetBlobContainerClient("media");
            var blobInstance = containerInstance.GetBlobClient(file.FileName);
            var blobHttpHeaders = new BlobHttpHeaders
            {
                ContentType = file.ContentType
            };
            await blobInstance.UploadAsync(file.OpenReadStream(), new BlobUploadOptions
            {
                HttpHeaders = blobHttpHeaders
            });
            return blobInstance.Uri.ToString();
        }
    }

}
