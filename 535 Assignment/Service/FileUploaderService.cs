using Microsoft.AspNetCore.StaticFiles;

namespace _535_Assignment.Service
{
    public class FileUploaderService
    {
        string _uploadRootPath; 
        private readonly EncryptionService _encryptionService;

        public FileUploaderService(IWebHostEnvironment env, EncryptionService encryptionService)
        {
            _uploadRootPath = Path.Combine(env.WebRootPath, "Uploads");
            _encryptionService = encryptionService;
        }

        /// <summary>
        /// Calculate and return a unique file name (with the file extension)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string UniqueFileName(string fileName)
        {
            try
            {
                // Retrieve a reference to the 'Upload' folder
                DirectoryInfo dir = new DirectoryInfo(_uploadRootPath);

                if (!dir.EnumerateFiles().Any(c => c.Name.Equals(fileName)))
                {
                    return fileName;
                }

                string extension = fileName.Split('.').LastOrDefault();

                if (String.IsNullOrEmpty(extension))
                {
                    extension = "txt";
                }

                return $"{Guid.NewGuid()}.{extension}";

            }
            catch (Exception e)
            {
                return null;
            }
        }


        /// <summary>
        /// Write the file contents to disk - ensuring a unique file name. AES Encryption is applied.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task SaveFile(IFormFile file)
        {
            string fileName = UniqueFileName(file.FileName);

            byte[] fileContents;

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                fileContents = stream.ToArray();
            }

            var encryptedData = _encryptionService.EncryptByteArray(fileContents);

            using (var stream = new MemoryStream(encryptedData))
            {
                var targetFile = Path.Combine(_uploadRootPath, fileName);
               
                using (var fileStream = new FileStream(targetFile, FileMode.Create))
                {
                    stream.WriteTo(fileStream);
                }
            }

        }

        /// <summary>
        /// Loads and returns the file on disk, or null;
        /// </summary>
        /// <param name="fileName">the fully qualified (extension included) name of the file</param>
        /// <returns></returns>
        public FileInfo LoadFile(string fileName)
        {
            DirectoryInfo dir = new DirectoryInfo(_uploadRootPath);

            if (!dir.EnumerateFiles().Any(c => c.Name.Equals(fileName)))
            {
                return null;
            }

            return dir.EnumerateFiles().Where(c => c.Name.Equals(fileName)).FirstOrDefault();
        }


        /// <summary>
        /// Return the MIME type of given fileName (must include the extension)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetFileExtension(string fileName)
        {
            if (new FileExtensionContentTypeProvider().TryGetContentType(fileName, out string contentType))
            {
                return contentType;
            }

            return null;

        }

        /// <summary>
        /// Retrieve a byte[] that represents a files data
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<byte[]> ReadFileIntoMemory(string fileName)
        {

            var file = LoadFile(fileName);

            if (file == null)
            {
                return null;
            }

            using (var memStream = new MemoryStream())
            {
                using (var fileStream = File.OpenRead(file.FullName))
                {
                    fileStream.CopyTo(memStream);

                    var encryptedData = memStream.ToArray();

                    return _encryptionService.DecryptByteArray(encryptedData);
                }
            }
        }


        /// <summary>
        /// Retrieve the path of a file to be loaded, or null
        /// </summary>
        /// <param name="fileName">the fully qualified (extension included) name of the file</param>
        /// <returns></returns>
        public async Task<string> GetFilePath(string fileName)
        {
            var file = LoadFile(fileName);

            if (file == null)
            {
                return null;
            }

            var directory = file.Directory.Name;

            if (directory.Equals("Uploads"))
            {
                return $"/{directory}/{file.Name}";
            }
            else
            {
                return $"/Uploads/{directory}/{file.Name}";
            }

        }
    }
}
