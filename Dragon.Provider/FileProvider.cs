using Microsoft.AspNetCore.Http;

namespace Dragon.Provider
{
    public static class FileProvider
    {
        public static class ContentTypes
        {
            public const string Excel = "application/vnd.openxmlformats-officedocment.spreadsheetml.sheet";
            public const string Doc = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

            public const string Pdf = "application/pdf";
            public const string Html = "text/html";
            public const string Png = "image/png";
            public const string Jpg = "image/jpg";
            public const string Jpeg = "image/jpeg";

            public const string General = "application/octet-stream";
            //public const string Zip = "application/zip";
            //public const string Json = "application/json";
            //public const string Vcard = "text/x-vcard";
        }
        public static class FileExtensions
        {
            public const string Pdf = ".pdf";
            public const string Xlsx = ".xlsx";
            public const string Xls = ".xls";
            public const string Doc = ".doc";
            public const string Docx = ".docx";
            public const string Html = ".html";
            public const string Png = ".png";
            public const string Jpg = ".jpg";
            public const string Jpeg = ".jpeg";

            //public const string Zip = ".zip";
            //public const string Json = ".json";
        }
        public static string ContentType(string fileName)
        {
            string contentType = ContentTypes.General;
            switch (Path.GetExtension(fileName))
            {
                //case FileExtensions.Zip: { contentType = ContentTypes.Zip; break; }
                //case FileExtensions.Json: { contentType = ContentTypes.Json; break; }

                case FileExtensions.Xls:
                case FileExtensions.Xlsx: { contentType = ContentTypes.Excel; break; }
                case FileExtensions.Pdf: { contentType = ContentTypes.Pdf; break; }
                case FileExtensions.Html: { contentType = ContentTypes.Html; break; }
                case FileExtensions.Png: { contentType = ContentTypes.Png; break; }
                case FileExtensions.Jpg: { contentType = ContentTypes.Jpg; break; }
                case FileExtensions.Jpeg: { contentType = ContentTypes.Jpeg; break; }
                default: { break; }
            }
            return contentType;
        }

        public static async Task<string> ReadFileToPath(IFormFile file, string uploadPath)
        {
            PathProvider.CreateDirectory(uploadPath);
            string fileName = $"{Guid.NewGuid()}-{DateTime.Now:dd-MM-yyyy}{Path.GetExtension(file.FileName)}".Replace(" ", "");
            if (file.Length > 0)
            {
                FileStream filestream = new(Path.Combine(uploadPath, fileName), FileMode.Create, FileAccess.Write, FileShare.None);
                await file.CopyToAsync(filestream); await filestream.DisposeAsync();
            }
            return fileName;
        }
        public static void CopyTempFileToPath(string copyFileName, string copyToPath, string newFileName)
        {
            PathProvider.CreateDirectory(copyToPath);
            File.Copy(Path.Combine(ConfigProvider.Provider.BaseDirectory, ConfigProvider.Settings.TempFolderName, copyFileName), Path.Combine(copyToPath, newFileName));
        }
    }
}
