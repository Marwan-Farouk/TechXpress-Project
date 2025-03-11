using Microsoft.AspNetCore.Http;

namespace BusinessLayer.Services
{
    public class FileService
    {
        public string UploadFile(IFormFile file, string destinationFolder)
        {
            var uniqueFileName = string.Empty;

            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(@"./wwwroot/", destinationFolder);   

                uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;  

                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

            }
            return uniqueFileName;  
        }
    }
}
