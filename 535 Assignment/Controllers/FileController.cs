using _535_Assignment.Service;
using Microsoft.AspNetCore.Mvc;

namespace _535_Assignment.Controllers
{
    public class FileController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FileUploaderService _uploader;

        public FileController(ILogger<HomeController> logger, FileUploaderService uploader)
        {
            _uploader = uploader;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }



        /// <summary>
        /// Handle the Form posting of the uploaded file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ImageUpload(IFormFile file)
        {
            var validationResult = ValidateFileUpload(file);

            if (validationResult != null && validationResult.Count > 0)
            {
                foreach (var error in validationResult)
                {
                    ModelState.AddModelError("UploadError", error);
                }
                return View("Index");
            }

            await _uploader.SaveFile(file);
            return View("Index");
        }

        private List<string> ValidateFileUpload(IFormFile file)
        {
            List<string> errors = new List<string>();
            if (file.Length > 10000000)
            {
                errors.Add("File exceeds the 10MB size limit");
            }

            if (file.FileName.Contains('.'))
            {
                string[] acceptableExtensions = { "png", "bmp", "jpg", "jpeg" };
                string extension = file.FileName.Split('.').LastOrDefault();
                if (extension == null)
                {
                    errors.Add("File does not have an acceptable extension");
                }
                else
                {
                    if (!acceptableExtensions.Any(c => c.Equals(extension)))
                    {
                        errors.Add($"The file extension of {extension} is not allowed");
                    }
                }
            }

            return errors;

        }

        /// <summary>
        /// Find and return the path to the selected image to the View
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> LoadImage(string fileName)
        {
            byte[] fileBytes = await _uploader.ReadFileIntoMemory(fileName);
            var imageData = System.Convert.ToBase64String(fileBytes);

            ViewData["ImageSource"] = $"data:image/png;base64,{imageData}";
            ViewData["ImageAlt"] = "Image Loaded";
            return View("Index");

        }

        /// <summary>
        /// Download a file based on the filename
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            byte[] fileBytes = await _uploader.ReadFileIntoMemory(fileName);

            if (fileBytes == null || fileBytes.Length == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            return File(fileBytes, "application/octet-stream", fileDownloadName: fileName);

        }



    }
}
