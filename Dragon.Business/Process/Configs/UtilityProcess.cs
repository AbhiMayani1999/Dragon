using Dragon.Data;
using Dragon.Enm;
using Dragon.Model;
using Dragon.Model.Configs;
using Dragon.Provider;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static Dragon.Provider.ConnectionProvider;
using static Dragon.Provider.ConnectionProvider.ContextProvider;

namespace Dragon.Business.Process.Configs
{
    public class UtilityProcess
    {
        public static async Task<ApiResponse> SeedAllDatabase()
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Failed };
            try
            {
                List<Options> resultList = [];
                foreach (Connection connection in ConnectionProvider.Provider.GetAllConnection()) { resultList.Add(new() { Key = connection.Database, Value = await SeedTenant(connection) ? "Success" : "Failed" }); }
                apiResponse = new ApiResponse { Data = resultList, Status = (byte)StatusFlags.Success };
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), null); apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
        public static async Task<ApiResponse> GenerateAllPages()
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Failed };
            try
            {
                List<Options> resultList = [];
                foreach (Connection connection in ConnectionProvider.Provider.GetAllConnection()) { resultList.Add(new() { Key = connection.Database, Value = await GeneratePages(connection) ? "Success" : "Failed" }); }
                apiResponse = new ApiResponse { Data = resultList, Status = (byte)StatusFlags.Success };
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), null); apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
        public static async Task<ApiResponse> BackupAllDatabase()
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Failed };
            try
            {
                List<Options> resultList = [];
                foreach (Connection connection in ConnectionProvider.Provider.GetAllConnection()) { resultList.Add(new() { Key = connection.Database, Value = await BackupTenant(connection) ? "Success" : "Failed" }); }
                apiResponse = new ApiResponse { Data = resultList, Status = (byte)StatusFlags.Success };
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), null); apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
        public static async Task<ApiResponse> MigrateAllDatabase()
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Failed };
            try
            {
                List<Options> resultList = [];
                foreach (Connection connection in ConnectionProvider.Provider.GetAllConnection()) { resultList.Add(new() { Key = connection.Database, Value = await MigrateTenant(connection) ? "Success" : "Failed" }); }
                apiResponse = new ApiResponse { Data = resultList, Status = (byte)StatusFlags.Success };
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), null); apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
        public static async Task<ApiResponse> MasterDomainConfig(string data)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Failed };
            try
            {
                if (!string.IsNullOrWhiteSpace(data))
                {
                    DefaultContext defaultContext = new(ConnectionProvider.Provider.GetConnection(ConfigProvider.MasterTenantName));
                    apiResponse = new ApiResponse { Data = await defaultContext.DomainConnect.FirstOrDefaultAsync(d => d.Name == data), Status = (byte)StatusFlags.Success };
                }
                else { apiResponse.Message = "Configuration not found"; }
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), data); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }

        public static async Task<bool> BackupTenant(Connection data)
        {
            bool isSuccess;
            try
            {
                switch ((DatabaseType)data.DatabaseType)
                {
                    case DatabaseType.SqlServer: { await BackupMSSQLDatabase(data); break; }
                    default: { await BackupMSSQLDatabase(data); break; }
                }
                isSuccess = true;
            }
            catch (Exception) { throw; }
            return isSuccess;
        }
        public static async Task<bool> MigrateTenant(Connection data)
        {
            bool isSuccess;
            try
            {
                switch ((DatabaseType)data.DatabaseType)
                {
                    case DatabaseType.MySql: { using MySqlContext defaultContext = new(data); await defaultContext.Database.MigrateAsync(); break; }
                    case DatabaseType.SqlServer: { using SqlServerContext defaultContext = new(data); await defaultContext.Database.MigrateAsync(); break; }
                    default: { using SqlServerContext defaultContext = new(data); await defaultContext.Database.MigrateAsync(); break; }
                }
                isSuccess = true;
            }
            catch (Exception) { throw; }
            return isSuccess;
        }
        public static async Task<bool> BackupMSSQLDatabase(Connection connection)
        {
            bool isSuccess = false;
            try
            {
                using SqlConnection sqlConnection = new(ConnectionBuilder.GetSqlServerConnection(connection));
                ConfigProvider.Provider.CreateTenantFolderStructure(connection.TenantCode);
                string backupData = Path.Combine(ConfigProvider.Provider.BaseDirectory, connection.TenantCode, ConfigProvider.Settings.DatabaseBackupFolderName,
                                                 $"{connection.Database}-{DateTime.Now:dd-MM-yyyy-hh-mm-tt}.bak");
                using SqlCommand sqlCommand = new($"Backup Database [{connection.Database}] TO DISK='{backupData}'", sqlConnection);
                if (sqlConnection.State == ConnectionState.Closed) { sqlConnection.Open(); }
                await sqlCommand.ExecuteNonQueryAsync(); sqlConnection.Close();
                isSuccess = true;
            }
            catch (Exception) { throw; }
            return isSuccess;
        }

        public static async Task<bool> SeedTenant(Connection data)
        {
            bool isSuccess;
            try
            {
                //if (data.TenantCode != ConfigProvider.MasterTenantName)
                //{
                await MigrateTenant(data);
                await SeedDatabase(data);
                isSuccess = true;
                //}
            }
            catch (Exception) { throw; }
            return isSuccess;
        }
        public static async Task<bool> SeedDatabase(Connection data)
        {
            bool isSuccess = false;
            try
            {
                DefaultContext defaultContext = new(data);

                await SeedComponentProperty(defaultContext);

                await SeedUserType(defaultContext, new UserType { Name = "Admin", IsAdmin = true, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now });
                await SeedUserType(defaultContext, new UserType { Name = "User", IsAdmin = true, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now });
                await defaultContext.SaveChangesAsync();

                await SeedUser(defaultContext, new User { Username = "master@kautilyam.com", Password = EncryptionProvider.Encrypt("Master"), UserTypeId = (await defaultContext.UserType.AsNoTracking().FirstOrDefaultAsync(d => d.Name == "Admin")).Id, IsUniversal = true });
                await SeedUser(defaultContext, new User { Username = "admin@kautilyam.com", Password = EncryptionProvider.Encrypt("Admin"), UserTypeId = (await defaultContext.UserType.AsNoTracking().FirstOrDefaultAsync(d => d.Name == "Admin")).Id });
                await defaultContext.SaveChangesAsync();

                await SeedConnection(defaultContext, new Connection { TenantCode = "Kautilyam", DatabaseType = (int)DatabaseType.SqlServer, Database = "DragonKautilyam", Server = ".", User = "sa", Password = "Krishna@003" });
                await defaultContext.SaveChangesAsync();

                isSuccess = true;
            }
            catch (Exception) { throw; }
            return isSuccess;
        }

        public static async Task SeedUser(DefaultContext context, User data)
        {
            if (!await context.User.AnyAsync(d => d.Username == data.Username)) { await context.User.AddAsync(data); }
        }
        public static async Task SeedConnection(DefaultContext context, Connection data)
        {
            if (!await context.Connection.AnyAsync(d => d.TenantCode == data.TenantCode)) { await context.Connection.AddAsync(data); }
        }
        public static async Task SeedUserType(DefaultContext context, UserType data)
        {
            if (!await context.UserType.AnyAsync(d => d.Name == data.Name)) { await context.UserType.AddAsync(data); }
        }

        public static async Task SeedComponentProperty(DefaultContext defaultContext)
        {
            await SeedComponentProperty(defaultContext, new ComponentProperty { Name = "State", IsGeneric = true });
            await SeedComponentProperty(defaultContext, new ComponentProperty { Name = "Protected", IsGeneric = true });
            await SeedComponentProperty(defaultContext, new ComponentProperty { Name = "Validation", IsGeneric = true });
            await SeedComponentProperty(defaultContext, new ComponentProperty { Name = "Title", IsGeneric = true });
            await SeedComponentProperty(defaultContext, new ComponentProperty { Name = "Type", IsGeneric = true });
            await SeedComponentProperty(defaultContext, new ComponentProperty { Name = "Index", IsGeneric = true });
            await SeedComponentProperty(defaultContext, new ComponentProperty { Name = "Checked", IsGeneric = true });
            await SeedComponentProperty(defaultContext, new ComponentProperty { Name = "JsonProperty", IsGeneric = true });
            await SeedComponentProperty(defaultContext, new ComponentProperty { Name = "Value", IsGeneric = true });
            await SeedComponentProperty(defaultContext, new ComponentProperty { Name = "Message", IsGeneric = true });
            await SeedComponentProperty(defaultContext, new ComponentProperty { Name = "Id", IsGeneric = true });
            await SeedComponentProperty(defaultContext, new ComponentProperty { Name = "Identifier", IsGeneric = true });
            await SeedComponentProperty(defaultContext, new ComponentProperty { Name = "Fields", IsGeneric = true });
            await SeedComponentProperty(defaultContext, new ComponentProperty { Name = "Widgets", IsGeneric = true });
            await SeedComponentProperty(defaultContext, new ComponentProperty { Name = "Form", IsGeneric = true });
            await SeedComponentProperty(defaultContext, new ComponentProperty { Name = "Dataurl", IsGeneric = true });
            await SeedComponentProperty(defaultContext, new ComponentProperty { Name = "Submiturl", IsGeneric = true });
            await SeedComponentProperty(defaultContext, new ComponentProperty { Name = "Options", IsGeneric = true });
            await SeedComponentProperty(defaultContext, new ComponentProperty { Name = "ServerSidePagination", IsGeneric = true });
            await SeedComponentProperty(defaultContext, new ComponentProperty { Name = "EditAction", IsGeneric = true });
            await SeedComponentProperty(defaultContext, new ComponentProperty { Name = "DeleteAction", IsGeneric = true });
            await SeedComponentProperty(defaultContext, new ComponentProperty { Name = "Table", IsGeneric = true });
            await SeedComponentProperty(defaultContext, new ComponentProperty { Name = "Key", IsGeneric = true });
            await SeedComponentProperty(defaultContext, new ComponentProperty { Name = "KeyStore", IsGeneric = true });
            await SeedComponentProperty(defaultContext, new ComponentProperty { Name = "OptionTransfer", IsGeneric = true });
            await SeedComponentProperty(defaultContext, new ComponentProperty { Name = "NavigationUrl", IsGeneric = true });
            await defaultContext.SaveChangesAsync();

        }
        public static async Task SeedComponentProperty(DefaultContext context, ComponentProperty data)
        {
            if (!await context.ComponentProperty.AnyAsync(d => d.Name == data.Name)) { await context.ComponentProperty.AddAsync(data); }
        }

        public static async Task<bool> GeneratePages(Connection data)
        {
            bool isSuccess = false;
            try
            {
                Console.WriteLine("Generating Pages");
                StructureProcess process = new() { CurrentUser = new User { TenantCode = data.TenantCode } };
                using DefaultContext defaultContext = new(data);

                List<Navigation> navigationList = await defaultContext.Navigation.ToListAsync();
                List<Structure> structureList = (List<Structure>)(await process.Get(0)).Data;
                List<StructurePropertyToStructure> structureMappingList = await defaultContext.StructurePropertyToStructure.AsNoTracking().ToListAsync();
                GenerateJson(navigationList, structureList, structureMappingList, process.PageConfigDirectory, null);
                isSuccess = true;
                Console.WriteLine("Pages Generation Successfull");
            }
            catch (Exception) { throw; }
            return isSuccess;
        }
        public static object CreateJson(Structure structure, List<Structure> structureList, List<StructurePropertyToStructure> structurePropertyToStoreMapping)
        {
            Dictionary<string, object> dataDir = [];
            foreach (StructureProperty property in structure.Properties)
            {
                object value = "";
                if (property.ComponentStructureProperty.IsMultiple)
                {
                    List<Dictionary<string, object>> subDirectory = [];
                    structureList.Where(d => property.ChildMapping.Any(l => d.Id == l)).ToList().ForEach(subStructure =>
                    { subDirectory.Add(new() { { subStructure.ComponentStructure.Name, CreateJson(subStructure, structureList, structurePropertyToStoreMapping) } }); });
                    value = subDirectory;
                }
                else if (structurePropertyToStoreMapping.Any(d => d.StructurePropertyId == property.Id))
                {
                    StructurePropertyToStructure propertyToStore = structurePropertyToStoreMapping.FirstOrDefault(d => d.StructurePropertyId == property.Id);
                    if (propertyToStore != null) { value = CreateJson(structureList.FirstOrDefault(d => d.Id == propertyToStore.ChildStructureId), structureList, structurePropertyToStoreMapping); }
                }
                else if (!string.IsNullOrWhiteSpace(property.Value))
                {
                    value = property.Type == typeof(bool).Name && bool.TryParse(property.Value, out bool parsedValue) ? parsedValue : property.Value;
                }
                if (!dataDir.Any(d => d.Key == property.ComponentStructureProperty.ComponentProperty.Name))
                {
                    dataDir.Add(property.ComponentStructureProperty.ComponentProperty.Name, value);
                }
                else
                {
                    Console.WriteLine("Same Property");
                }
            }
            return dataDir.ToJson().FromJson<object>();
        }
        public static void GenerateJson(List<Navigation> navigationList, List<Structure> structureList, List<StructurePropertyToStructure> structureMappingList, string pageConfigDirectory, int? parentNavigationId)
        {
            navigationList.Where(d => d.ParentNavigationId == parentNavigationId).ToList().ForEach(navigation =>
            {
                structureList.ToList().ForEach(structure =>
                {
                    StructureProperty stateProperty = structure.Properties.FirstOrDefault(d => d.ComponentStructureProperty.ComponentProperty.Name == "State");
                    if (stateProperty != null && stateProperty.Value == navigation.State)
                    {
                        JsonProvider.ToJsonFile(CreateJson(structure, structureList, structureMappingList), Path.Combine(pageConfigDirectory, $"{navigation.State}.json"));
                        Console.WriteLine($"Generated Page: {navigation.Name}");
                    }
                });
                if (navigationList.Any(d => d.ParentNavigationId == navigation.Id))
                {
                    PathProvider.CreateDirectory(Path.Combine(pageConfigDirectory, navigation.State)); GenerateJson(navigationList, structureList, structureMappingList, Path.Combine(pageConfigDirectory, navigation.State), navigation.Id);
                }
            });
        }
    }

    public class JobProcess : GlobalVariables
    {
        public async Task<ApiResponse> Migrate()
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Failed };
            try
            {
                if (await UtilityProcess.MigrateTenant(GetConnection()))
                {
                    apiResponse.Status = (byte)StatusFlags.Success;
                }
                else { apiResponse.Message = "Database migration failed"; }
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), null); apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
        public async Task<ApiResponse> Backup()
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Failed };
            try
            {
                if (await UtilityProcess.BackupTenant(GetConnection()))
                {
                    apiResponse.Status = (byte)StatusFlags.Success;
                }
                else { apiResponse.Message = "Database backup failed"; }
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), null); apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
        public async Task<ApiResponse> Pages()
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Failed };
            try
            {
                if (await UtilityProcess.GeneratePages(GetConnection()))
                {
                    apiResponse.Status = (byte)StatusFlags.Success;
                }
                else { apiResponse.Message = "Pages generation failed"; }
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), null); apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
    }
}
//public static List<StructureStoreProperty> FillComponentPropertyStore(List<ComponentStructureProperty> data)
//{
//    return data.Select(d => new StructureStoreProperty() { ComponentStructurePropertyId = d.Id, Type = typeof(string).Name, Value = d.ComponentProperty.Name }).ToList();
//}
//public static StructureStore FillStore(string globleIdentifier, ComponentStructure structure)
//{
//    List<StructureStore> subStructures = new();
//    if (structure.SubComponents != null && structure.SubComponents.Count > 0)
//    {
//        subStructures = structure.SubComponents.Select(d => FillStore(globleIdentifier, d)).ToList();
//    }
//    List<StructureStoreProperty> properties = new();
//    if (structure.Properties != null && structure.Properties.Count > 0)
//    {
//        properties = FillComponentPropertyStore(structure.Properties);
//    }

//    return new()
//    {
//        Identifier = globleIdentifier,
//        ComponentStructureId = structure.Id,
//        Properties = properties,
//        SubStructures = subStructures
//    };
//}

//public static async Task SeedConnection(DefaultContext context, Connection data)
//{
//    if (await context.Connection.CountAsync(d => d.TenantCode == data.TenantCode) == 0) {  await context.Connection.AddAsync(data); }
//}

//public static async Task SeedFormComponentStructure(DefaultContext context, List<string> data)
//{
//    foreach (string type in data)
//    {
//        await SeedComponentStructure(context, new()
//        {
//            Name = type,
//            IsGeneric = true,
//            ParentStructureName = "Form",
//            PropertyNameList = new() { "Id", "Title", "Type", "Index", "JsonProperty", "Validation" }
//        });
//    }
//}
//public static async Task SeedTableComponentStructure(DefaultContext context, List<string> data)
//{
//    foreach (string type in data)
//    {
//        await SeedComponentStructure(context, new()
//        {
//            Name = type,
//            IsGeneric = true,
//            ParentStructureName = "Table",
//            PropertyNameList = new() { "Id", "Title", "Type", "Index", "JsonProperty" }
//        });
//    }
//}
//public static async Task SeedComponentStructure(DefaultContext context, ComponentStructure data)
//{
//    if (await context.ComponentStructure.CountAsync(d => d.Name == data.Name) == 0)
//    {
//        if (!string.IsNullOrWhiteSpace(data.ParentStructureName))
//        {
//            data.SubComponents.Add(new() { ParentComponentId = (await context.ComponentStructure.AsNoTracking().FirstAsync(d => d.Name == data.ParentStructureName)).Id });
//        }
//        if (data.PropertyNameList != null && data.PropertyNameList.Count > 0)
//        {
//            data.Properties = new();
//            data.Properties.AddRange(await context.ComponentProperty.AsNoTracking().Where(d => data.PropertyNameList.Any(p => p == d.Name))
//                .Select(d => new ComponentStructureProperty() { ComponentPropertyId = d.Id, IsMultiple = d.Name == "Validation" || d.Name == "Fields" }).ToListAsync());
//        }
//         await context.ComponentStructure.AddAsync(data);
//    }
//}

//await SeedConnection(defaultContext, new Connection { TenantCode = "King", DatabaseType = 1, Server = ".", Password = "Krishna@003", User = "sa" });
//await SeedConnection(defaultContext, new Connection { TenantCode = "Cobra", DatabaseType = 2, Server = ".", Password = "Krishna@003", User = "sa" });
//await SeedConnection(defaultContext, new Connection { TenantCode = "Margrita", DatabaseType = 2, Server = ".", Password = "Krishna@003", User = "sa" });
//await SeedConnection(defaultContext, new Connection { TenantCode = "Demo", DatabaseType = 3, Server = ".", Password = "Krishna@003", User = "sa" });
//await SeedConnection(defaultContext, new Connection { TenantCode = "Development", DatabaseType = 2, Server = ".", Password = "Krishna@003", User = "sa" });
//await SeedConnection(defaultContext, new Connection { TenantCode = "QA", DatabaseType = 3, Server = ".", Password = "Krishna@003", User = "sa" });
//await SeedConnection(defaultContext, new Connection { TenantCode = "Stage", DatabaseType = 1, Server = ".", Password = "Krishna@003", User = "sa" });
//await SeedConnection(defaultContext, new Connection { TenantCode = "Prod", DatabaseType = 1, Server = ".", Password = "Krishna@003", User = "sa" });
// await defaultContext.SaveChangesAsync();
//await SeedComponentStructure(defaultContext, new() { Name = "Page", IsGeneric = true, PropertyNameList = new() { "Title", "Index", "State", "Protected" } });
// await defaultContext.SaveChangesAsync();

//await SeedComponentStructure(defaultContext, new() { Name = "Form", IsGeneric = true, ParentStructureName = "Page", PropertyNameList = new() { "Title", "Fields", "Identifier", "Submiturl" } });
//await SeedComponentStructure(defaultContext, new() { Name = "Table", IsGeneric = true, ParentStructureName = "Page" });
// await defaultContext.SaveChangesAsync();

//await SeedComponentStructure(defaultContext, new() { Name = "Validation", IsGeneric = true, ParentStructureName = "Form" });
// await defaultContext.SaveChangesAsync();

//await SeedComponentStructure(defaultContext, new() { Name = "Required", IsGeneric = true, ParentStructureName = "Validation", PropertyNameList = new() { "Message" } });
//await SeedComponentStructure(defaultContext, new() { Name = "Email", IsGeneric = true, ParentStructureName = "Validation", PropertyNameList = new() { "Message" } });
//await SeedComponentStructure(defaultContext, new() { Name = "Pattern", IsGeneric = true, ParentStructureName = "Validation", PropertyNameList = new() { "Value", "Message" } });
//await SeedComponentStructure(defaultContext, new() { Name = "Min", IsGeneric = true, ParentStructureName = "Validation", PropertyNameList = new() { "Value", "Message" } });
//await SeedComponentStructure(defaultContext, new() { Name = "Max", IsGeneric = true, ParentStructureName = "Validation", PropertyNameList = new() { "Value", "Message" } });
//await SeedComponentStructure(defaultContext, new() { Name = "Minlength", IsGeneric = true, ParentStructureName = "Validation", PropertyNameList = new() { "Value", "Message" } });
//await SeedComponentStructure(defaultContext, new() { Name = "Maxlength", IsGeneric = true, ParentStructureName = "Validation", PropertyNameList = new() { "Value", "Message" } });
// await defaultContext.SaveChangesAsync();

//await SeedFormComponentStructure(defaultContext, new() { "Text", "Date", "Email", "Number", "Password", "Textarea", "Htmleditor", "Switch", "Checkbox", "Select", "Multiselect", "Autocomplete", "Iconselect" });
//await SeedTableComponentStructure(defaultContext, new() { "Text", "Date", "Number", "Boolean", "Icon" });
// await defaultContext.SaveChangesAsync();

//ComponentStructure PageComponent = await defaultContext.ComponentStructure.AsNoTracking().FirstAsync(d => d.Name == "Page");
//ComponentStructure TableComponent = await defaultContext.ComponentStructure.AsNoTracking().Include(d => d.SubComponents).FirstAsync(d => d.Name == "Table");
//ComponentStructure FormComponent = await defaultContext.ComponentStructure.AsNoTracking()
//                                    .Include(d => d.Properties)
//                                        .ThenInclude(d => d.ComponentProperty)
//                                    .Include(d => d.SubComponents)
//                                        .Include(d => d.Properties)
//                                            .ThenInclude(d => d.ComponentProperty)
//                                    .FirstAsync(d => d.Name == "Form");
//StructureStore store = FillStore("SampleForm", FormComponent);
// await defaultContext.StructureStore.AddAsync(store);
// defaultContext.SaveChanges();