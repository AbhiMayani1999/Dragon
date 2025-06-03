namespace Dragon.Provider
{
    public sealed class ConfigProvider
    {
        private const string _configFile = "appsettings.json";
        private ApplicationConfig _appConfig = new();
        private static readonly Lazy<ConfigProvider> _constantsProvider = new(() => new ConfigProvider());
        public static ConfigProvider Provider => _constantsProvider.Value;
        public static ApplicationSettings Settings => _constantsProvider.Value._appConfig.AppSettings;

        private ConfigProvider()
        {
            if (ReadAppSettings())
            {
                CreateFolderStructure(MasterTenantName);
                PathProvider.CreateDirectory(Path.Combine(BaseDirectory, _appConfig.AppSettings.TempFolderName));
                PathProvider.CreateDirectory(Path.Combine(BaseDirectory, _appConfig.AppSettings.BrowserFolderName));
                PathProvider.CreateDirectory(Path.Combine(BaseDirectory, _appConfig.AppSettings.LogsFolderName));
            }
        }
        private bool ReadAppSettings()
        {
            bool isSuccess = false;
            if (!File.Exists(_configFile)) { throw new FileNotFoundException(); }
            else
            {
                using StreamReader streamReader = new(_configFile);
                string dataStream = streamReader.ReadToEnd();
                if (!string.IsNullOrWhiteSpace(dataStream))
                {
                    _appConfig = dataStream.FromJson<ApplicationConfig>();
                    _appConfig ??= new();
                    isSuccess = true;
                }
                else { throw new ArgumentNullException(); }
                streamReader.Dispose();
            }
            return isSuccess;
        }
        private void CreateFolderStructure(string tenantName)
        {
            PathProvider.CreateDirectory(Path.Combine(BaseDirectory, tenantName, _appConfig.AppSettings.HtmlFolderName));
            PathProvider.CreateDirectory(Path.Combine(BaseDirectory, tenantName, _appConfig.AppSettings.PdfFolderName));
            PathProvider.CreateDirectory(Path.Combine(BaseDirectory, tenantName, _appConfig.AppSettings.ImageFolderName));
            PathProvider.CreateDirectory(Path.Combine(BaseDirectory, tenantName, _appConfig.AppSettings.DocumentFolderName));
            PathProvider.CreateDirectory(Path.Combine(BaseDirectory, tenantName, _appConfig.AppSettings.JsonBackupFolderName));
            PathProvider.CreateDirectory(Path.Combine(BaseDirectory, tenantName, _appConfig.AppSettings.SpreadsheetFolderName));
            PathProvider.CreateDirectory(Path.Combine(BaseDirectory, tenantName, _appConfig.AppSettings.PageConfigFolderName));
            PathProvider.CreateDirectory(Path.Combine(BaseDirectory, tenantName, _appConfig.AppSettings.DatabaseBackupFolderName));
        }
        public void CreateTenantFolderStructure(string tenantName)
        {
            PathProvider.CreateDirectory(Path.Combine(BaseDirectory, tenantName));
            CreateFolderStructure(tenantName);
        }
        public static string EncryptionKey => "b14ca5898a4e4133bbce2ea2315a1916";
        public static string MasterTenantName => "Master";
        public string BaseDirectory => Path.Combine(@"C:/", _appConfig.AppSettings.BaseFolderName);
        public string LogsDirectory => Path.Combine(BaseDirectory, Settings.LogsFolderName);
        public string TempDirectory => Path.Combine(BaseDirectory, Settings.TempFolderName);
        public string BrowserDirectory => Path.Combine(BaseDirectory, Settings.BrowserFolderName);

        internal class ApplicationConfig { public ApplicationSettings AppSettings { get; set; } }
        public class ApplicationSettings
        {
            public string BaseFolderName { get; set; }
            public string TempFolderName { get; set; }
            public string LogsFolderName { get; set; }
            public string BrowserFolderName { get; set; }

            public string LightDataExtension { get; set; }
            public object MasterConnection { get; set; }

            public string ImageFolderName { get; set; }
            public string HtmlFolderName { get; set; }
            public string SpreadsheetFolderName { get; set; }
            public string PdfFolderName { get; set; }

            public string DocumentFolderName { get; set; }
            public string DatabaseBackupFolderName { get; set; }

            public string JsonBackupFolderName { get; set; }
            public string PageConfigFolderName { get; set; }
        }
    }
}