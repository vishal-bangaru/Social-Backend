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
    
        public interface FileServiceInterface
        {
            Task<string> Upload(IFormFile file);
        }

    
}
