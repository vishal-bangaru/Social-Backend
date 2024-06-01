using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
namespace Common.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(string cloudName, string apiKey, string apiSecret)
        {
            _cloudinary = new Cloudinary(new Account(cloudName, apiKey, apiSecret));
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            try
            {
                
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("File is null or empty.");
                }

                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Folder = "posts" // Specify the folder where you want to upload the file
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);


                    Console.WriteLine(uploadResult.SecureUrl.ToString());
                    return uploadResult.SecureUrl.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error uploading file to Cloudinary.", ex);
            }
        }
    }
}