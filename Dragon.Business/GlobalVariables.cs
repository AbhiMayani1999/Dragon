using Dragon.Data;
using Dragon.Enm;
using Dragon.Model;
using Dragon.Model.Configs;
using Dragon.Provider;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using static Dragon.Provider.ConnectionProvider;
using static Dragon.Provider.FileProvider;

namespace Dragon.Business
{
    public class GlobalVariables : IDisposable
    {
        public User CurrentUser { get; set; }
        internal string TenantCode => CurrentUser.TenantCode;
        public void Dispose() { GC.SuppressFinalize(this); }


        internal T UpdateTransection<T>(T data)
        {
            if (CurrentUser != null && CurrentUser.Id != 0)
            {
                PropertyInfo createdByField = typeof(T).GetProperty(nameof(TransectionKeys.CreatedBy));
                string createdByValue = Convert.ToString(createdByField.GetValue(data));
                if (string.IsNullOrWhiteSpace(createdByValue) || createdByValue == "0")
                {
                    PropertyInfo createdDateField = typeof(T).GetProperty(nameof(TransectionKeys.CreatedDate));
                    createdByField.SetValue(data, CurrentUser.Id);
                    createdDateField.SetValue(data, DateTime.Now);
                }
                else
                {
                    PropertyInfo updatedByField = typeof(T).GetProperty(nameof(TransectionKeys.UpdatedBy));
                    PropertyInfo updatedDateField = typeof(T).GetProperty(nameof(TransectionKeys.UpdatedDate));
                    updatedByField.SetValue(data, CurrentUser.Id);
                    updatedDateField.SetValue(data, DateTime.Now);
                }
            }
            return data;
        }
        internal T FileUpload<T>(T data, string propertyName, string existingFile)
        {
            string fileUrl = Convert.ToString(data.GetPropertyValue(propertyName));
            if (existingFile != fileUrl)
            {
                if (!string.IsNullOrWhiteSpace(fileUrl))
                {
                    string fileName = $"{typeof(T).Name}-{propertyName}-{DateTime.Now:dd-MM-yyyy-hh-mm-ss-tt}{Path.GetExtension(fileUrl)}".ToLower();
                    MoveTempFileToPath(Path.Combine(TempDirectory, fileUrl), ImageDirectory, fileName);
                    data.SetPropertyValue(propertyName, fileName);
                }
                else { data.SetPropertyValue(propertyName, string.Empty); if (!string.IsNullOrWhiteSpace(existingFile)) { PathProvider.DeleteFile(Path.Combine(ImageDirectory, existingFile)); } }
                if (!string.IsNullOrWhiteSpace(existingFile) && existingFile != fileUrl) { PathProvider.DeleteFile(Path.Combine(ImageDirectory, existingFile)); }
            }
            return data;
        }
        internal Connection GetConnection() { return ConnectionProvider.Provider.GetConnection(TenantCode); }
        internal static void MoveTempFileToPath(string copyFileName, string copyToPath, string newFileName)
        {
            PathProvider.CreateDirectory(copyToPath);
            File.Move(Path.Combine(ConfigProvider.Provider.TempDirectory, copyFileName), Path.Combine(copyToPath, newFileName));
        }

        internal static string LogsDirectory => ConfigProvider.Provider.LogsDirectory;
        internal static string TempDirectory => ConfigProvider.Provider.TempDirectory;
        internal static string BrowserDirectory => ConfigProvider.Provider.BrowserDirectory;
        internal string HtmlDirectory => Path.Combine(ConfigProvider.Provider.BaseDirectory, CurrentUser.TenantCode, ConfigProvider.Settings.HtmlFolderName);
        internal string ImageDirectory => Path.Combine(ConfigProvider.Provider.BaseDirectory, CurrentUser.TenantCode, ConfigProvider.Settings.ImageFolderName);
        internal string PdfDirectory => Path.Combine(ConfigProvider.Provider.BaseDirectory, CurrentUser.TenantCode, ConfigProvider.Settings.SpreadsheetFolderName);
        internal string PageConfigDirectory => Path.Combine(ConfigProvider.Provider.BaseDirectory, CurrentUser.TenantCode, ConfigProvider.Settings.PageConfigFolderName);
        internal string SpreadsheetDirectory => Path.Combine(ConfigProvider.Provider.BaseDirectory, CurrentUser.TenantCode, ConfigProvider.Settings.SpreadsheetFolderName);

        internal string DocumentDirectory => Path.Combine(ConfigProvider.Provider.BaseDirectory, CurrentUser.TenantCode, ConfigProvider.Settings.DocumentFolderName);
        internal string JsonDirectory => Path.Combine(ConfigProvider.Provider.BaseDirectory, CurrentUser.TenantCode, ConfigProvider.Settings.JsonBackupFolderName);
        internal string DatabaseBackupDirectory => Path.Combine(ConfigProvider.Provider.BaseDirectory, CurrentUser.TenantCode, ConfigProvider.Settings.DatabaseBackupFolderName);

        internal static PageData GetRecordsOfPage<T>(PageData pageData, IEnumerable<T> data) { pageData.RecordsCount = data.Count(); pageData.Data = pageData.IsClientSide ? data : data.Skip(pageData.CurrentPage > 0 ? pageData.CurrentPage - (1 * pageData.PageSize) : 0).Take(pageData.PageSize); return pageData; }
        internal static PageData GetDefaultPage<T>(PageData data) { return new PageData { PageSize = data.PageSize != 0 ? data.PageSize : 12, IsClientSide = data.IsClientSide, CurrentPage = data.CurrentPage != 0 ? data.CurrentPage : 0, RecordsCount = data.RecordsCount != 0 ? data.RecordsCount : 0, Filter = data.IsClientSide || data.Filter == null ? DictionaryFromType<T>() : data.Filter }; }
        private static Dictionary<string, object> DictionaryFromType<T>() { Dictionary<string, object> directory = []; foreach (PropertyInfo prop in typeof(T).GetProperties()) { directory.Add(prop.Name.ToCamelCase(), null); } return directory; }

        internal async Task<List<T>> GetDataList<T>(int id = 0) where T : class
        {
            Dictionary<string, object> filter = []; if (id != 0) { filter.Add(nameof(id), id); }
            using DefaultContext defaultContext = new(GetConnection());
            return await defaultContext.Set<T>().AsQueryable().AsNoTracking().OrFilter(filter).ToListAsync();
        }
        public async Task<ApiResponse> Get<T>(int id) where T : class
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try { apiResponse.Data = await GetDataList<T>(id); }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), null); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
        public async Task<ApiResponse> GetPage<T>(PageData pageData) where T : class
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                pageData = GetDefaultPage<Structure>(pageData); using DefaultContext defaultContext = new(GetConnection());
                apiResponse.Data = GetRecordsOfPage(pageData, await defaultContext.Set<T>().AsQueryable().AsNoTracking().OrFilter(pageData.Filter).ToListAsync());
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), null); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }

        internal T FileUpload<T>(T data, string propertyName, string existingFile, string namePrefix)
        {
            string fileUrl = Convert.ToString(data.GetPropertyValue(propertyName));
            if (Convert.ToInt32(data.GetPropertyValue(nameof(User.Id))) == 0 || existingFile != fileUrl)
            {
                if (!string.IsNullOrWhiteSpace(fileUrl))
                {
                    string fileName = $"{typeof(T).Name}-{namePrefix}-{DateTime.Now:dd-MM-yyyy-hh-mm-ss-tt}{Path.GetExtension(fileUrl)}".ToLower();
                    CopyTempFileToPath(fileUrl, GetFolderLocation(fileName), fileName);
                    data.SetPropertyValue(propertyName, fileName);
                }
                else { data.SetPropertyValue(propertyName, string.Empty); }
                if (!string.IsNullOrWhiteSpace(existingFile) && existingFile != fileUrl) { PathProvider.DeleteFile(Path.Combine(GetFolderLocation(existingFile), existingFile)); }
            }
            return data;
        }
        public string GetFolderLocation(string fileName)
        {
            string location = ContentTypes.General;
            switch (Path.GetExtension(fileName))
            {
                //case FileExtensions.Zip: { location = ContentTypes.Zip; break; }
                //case FileExtensions.Json: { location = ContentTypes.Json; break; }
                case FileExtensions.Xls:
                case FileExtensions.Xlsx: { location = SpreadsheetDirectory; break; }
                case FileExtensions.Pdf: { location = PdfDirectory; break; }
                case FileExtensions.Html: { location = HtmlDirectory; break; }
                case FileExtensions.Png:
                case FileExtensions.Jpg:
                case FileExtensions.Jpeg: { location = ImageDirectory; break; }
                default: { break; }
            }
            return location;
        }
    }
}


//namespace Microsoft.EntityFrameworkCore
//{
//    public static partial class CustomExtensions
//    {
//        private static readonly MethodInfo SetMethod = typeof(DbContext).GetMethod(nameof(DbContext.Set));

//        public static List<object> GetList(this DbContext context, Type entityType) => (List<object>)SetMethod.MakeGenericMethod(entityType).Invoke(context, null);
//        public static List<object> GetListByPage(this DbContext context, Type entityType, int pageSize, int pageNumber)
//        => ((List<object>)SetMethod.MakeGenericMethod(entityType).Invoke(context, null) as IQueryable<object>).Skip(pageNumber * pageSize).Take(pageSize).ToList();

//        public static List<T> GetList<T>(this DbContext context) => (List<T>)SetMethod.MakeGenericMethod(typeof(T)).Invoke(context, null);
//        public static List<T> GetListByPage<T>(this DbContext context, int pageSize, int pageNumber) => ((List<T>)SetMethod.MakeGenericMethod(typeof(T)).Invoke(context, null) as IQueryable<T>).Skip(pageNumber * pageSize).Take(pageSize).ToList();
//    }
//}