using Dragon.Data;
using Dragon.Enm;
using Dragon.Model;
using Dragon.Model.Configs;
using Dragon.Provider;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json.Linq;

namespace Dragon.Business.Process.Configs
{
    public class GeneratorProcess : GlobalVariables
    {
        public async Task<string> Export(string exportFolderName)
        {
            using DefaultContext defaultContext = new(GetConnection());
            string exportCompletePath = Path.Combine(JsonDirectory, exportFolderName);

            PathProvider.DeleteDirectory(exportCompletePath);
            PathProvider.CreateDirectory(exportCompletePath);
            (await GetDataList<ComponentProperty>()).ToList().ToJsonFile(Path.Combine(exportCompletePath, $"{nameof(ComponentProperty)}.json"));
            (await GetDataList<ComponentStructure>()).ToList().ToJsonFile(Path.Combine(exportCompletePath, $"{nameof(ComponentStructure)}.json"));
            (await GetDataList<ComponentStructureProperty>()).ToList().ToJsonFile(Path.Combine(exportCompletePath, $"{nameof(ComponentStructureProperty)}.json"));
            (await GetDataList<ComponentStructureSubComponent>()).ToList().ToJsonFile(Path.Combine(exportCompletePath, $"{nameof(ComponentStructureSubComponent)}.json"));

            (await GetDataList<Structure>()).ToList().ToJsonFile(Path.Combine(exportCompletePath, $"{nameof(Structure)}.json"));
            (await GetDataList<StructureSubStructure>()).ToList().ToJsonFile(Path.Combine(exportCompletePath, $"{nameof(StructureSubStructure)}.json"));
            (await GetDataList<StructureProperty>()).ToList().ToJsonFile(Path.Combine(exportCompletePath, $"{nameof(StructureProperty)}.json"));
            (await GetDataList<StructurePropertyToStructure>()).ToList().ToJsonFile(Path.Combine(exportCompletePath, $"{nameof(StructurePropertyToStructure)}.json"));

            return exportCompletePath;
        }

        public async Task<ApiResponse> Page(JObject data)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using DefaultContext defaultContext = new(GetConnection());
                using IDbContextTransaction transaction = defaultContext.Database.BeginTransaction();
                await GeneratePage(defaultContext, data);
                transaction.Commit();
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), data); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }

        private static async Task<bool> GenerateForm(DefaultContext defaultContext, object data)
        {
            ComponentStructure fromComponent = await defaultContext.ComponentStructure.AsNoTracking().Include(d => d.Properties).ThenInclude(d => d.ComponentProperty).FirstOrDefaultAsync(d => d.Name == "From");

            //foreach (JProperty property in data.Properties())
            //{
            //    string key = property.Name;
            //    JToken value = property.Value;
            //    // Access property value based on its type (string, number, etc.)
            //    Console.WriteLine($"Key: {key}, Value: {value}");
            //}
            return true;
        }
        private static async Task<bool> GenerateTable(DefaultContext defaultContext, object data)
        {
            ComponentStructure tableComponent = await defaultContext.ComponentStructure.AsNoTracking().Include(d => d.Properties).ThenInclude(d => d.ComponentProperty).FirstOrDefaultAsync(d => d.Name == "Table");
            return true;
        }
        private async Task<bool> GeneratePage(DefaultContext defaultContext, JObject data)
        {
            ComponentStructure pageComponent = await defaultContext.ComponentStructure.AsNoTracking().Include(d => d.Properties).ThenInclude(d => d.ComponentProperty).FirstOrDefaultAsync(d => d.Name == "Page");
            Structure pageStructureStore = new() { ComponentStructureId = pageComponent.Id, Identifier = $"{pageComponent.Name}-{RandomProvider.RandomString(5)}", Properties = pageComponent.Properties.Select(d => new StructureProperty { ComponentStructurePropertyId = d.Id, Type = nameof(String) }).ToList() };
            await defaultContext.Structure.AddAsync(pageStructureStore);
            await defaultContext.SaveChangesAsync();

            ComponentStructure tableComponent = await defaultContext.ComponentStructure.AsNoTracking().Include(d => d.Properties).ThenInclude(d => d.ComponentProperty).FirstOrDefaultAsync(d => d.Name == "Table");
            Structure tableStructureStore = new() { ComponentStructureId = tableComponent.Id, Identifier = $"{tableComponent.Name}-{RandomProvider.RandomString(5)}", ParentStructureId = pageStructureStore.Id, Properties = tableComponent.Properties.Select(d => new StructureProperty { ComponentStructurePropertyId = d.Id, Type = nameof(String) }).ToList() };
            await defaultContext.Structure.AddAsync(pageStructureStore);
            await defaultContext.SaveChangesAsync();

            ComponentStructure fromComponent = await defaultContext.ComponentStructure.AsNoTracking().Include(d => d.Properties).ThenInclude(d => d.ComponentProperty).FirstOrDefaultAsync(d => d.Name == "From");
            Structure formStructureStore = new() { ComponentStructureId = tableComponent.Id, Identifier = $"{fromComponent.Name}-{RandomProvider.RandomString(5)}", ParentStructureId = pageStructureStore.Id, Properties = tableComponent.Properties.Select(d => new StructureProperty { ComponentStructurePropertyId = d.Id, Type = nameof(String) }).ToList() };
            await defaultContext.Structure.AddAsync(pageStructureStore);
            await defaultContext.SaveChangesAsync();

            foreach (JProperty property in data.Properties())
            {
                string key = property.Name;
                JToken value = property.Value;
                Console.WriteLine($"Key: {key}, Value: {value}");
            }
            return true;
        }
    }
}
