using Dragon.Data;
using Dragon.Enm;
using Dragon.Model;
using Dragon.Model.Configs;
using Dragon.Provider;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Dragon.Business.Process.Configs
{
    public class ComponentStructureProcess : GlobalVariables
    {
        public async Task<ApiResponse> Get(int id)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                Dictionary<string, object> filter = []; if (id != 0) { filter.Add(nameof(id), id); }
                using DefaultContext defaultContext = new(GetConnection());
                List<ComponentStructure> listData = await defaultContext.ComponentStructure.AsNoTracking().Include(d => d.Properties).ThenInclude(d => d.ComponentProperty).OrFilter(filter).ToListAsync();
                List<ComponentStructureSubComponent> subComponentMapping = await defaultContext.ComponentStructureSubComponent.AsNoTracking().ToListAsync();

                listData.ForEach(d =>
                {
                    d.PropertyList = d.Properties.Select(p => new OptionGType<int, bool>() { Key = p.ComponentProperty.Id, Value = p.IsMultiple }).ToList();
                    d.ParentMapping = subComponentMapping.Where(c => c.ChildComponentId == d.Id).Select(c => c.ParentComponentId).ToList();
                    d.ChildMapping = subComponentMapping.Where(c => c.ParentComponentId == d.Id).Select(c => c.ChildComponentId).ToList();
                });
                apiResponse.Data = listData;
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), null); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
        public async Task<ApiResponse> Save(ComponentStructure data)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using DefaultContext defaultContext = new(GetConnection());
                using IDbContextTransaction transaction = defaultContext.Database.BeginTransaction();

                if (data.Properties != null)
                {
                    defaultContext.ComponentStructureProperty.RemoveRange(await defaultContext.ComponentStructureProperty.Where(d => d.ComponentStructureId == data.Id && !data.Properties.Select(p => p.ComponentPropertyId).Contains(d.ComponentPropertyId)).ToListAsync());
                    await defaultContext.SaveChangesAsync();
                }

                if (data.Id == 0 && !await defaultContext.ComponentStructure.AsNoTracking().AnyAsync(d => d.Name == data.Name)) { await defaultContext.ComponentStructure.AddAsync(data); }
                else if (data.Id != 0 && !await defaultContext.ComponentStructure.AsNoTracking().AnyAsync(d => d.Name == data.Name && d.Id != data.Id)) { defaultContext.ComponentStructure.Update(data); }
                else { apiResponse.Status = (byte)StatusFlags.AlreadyExists; }
                await defaultContext.SaveChangesAsync();

                if (data.ParentMapping != null)
                {
                    defaultContext.ComponentStructureSubComponent.RemoveRange(await defaultContext.ComponentStructureSubComponent.AsNoTracking().Where(d => d.ChildComponentId == data.Id).ToListAsync());
                    defaultContext.ComponentStructureSubComponent.AddRange(data.ParentMapping.Select(d => new ComponentStructureSubComponent() { Id = 0, ChildComponentId = data.Id, ParentComponentId = d }).ToList());
                    await defaultContext.SaveChangesAsync();
                }
                transaction.Commit();
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), data); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
        public async Task<ApiResponse> Delete(int id)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using DefaultContext defaultContext = new(GetConnection());
                ComponentStructure data = await defaultContext.ComponentStructure.AsNoTracking().FirstAsync(d => d.Id == id);
                if (data != null && !data.IsGeneric) { defaultContext.ComponentStructure.Remove(data); await defaultContext.SaveChangesAsync(); }
                else { apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.Message = data.IsGeneric ? $"Generic deletion is not possible" : ""; }
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), id); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
    }
    public class ComponentPropertyProcess : GlobalVariables
    {
        public async Task<ApiResponse> Save(ComponentProperty data)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using DefaultContext defaultContext = new(GetConnection());

                if (data.Id == 0 && !await defaultContext.ComponentProperty.AsNoTracking().AnyAsync(d => d.Name == data.Name)) { await defaultContext.ComponentProperty.AddAsync(data); }
                else if (data.Id != 0 && !await defaultContext.ComponentProperty.AsNoTracking().AnyAsync(d => d.Name == data.Name && d.Id != data.Id)) { defaultContext.ComponentProperty.Update(data); }
                else { apiResponse.Status = (byte)StatusFlags.AlreadyExists; }
                await defaultContext.SaveChangesAsync();
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), data); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
        public async Task<ApiResponse> Delete(int id)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using DefaultContext defaultContext = new(GetConnection());
                ComponentProperty data = await defaultContext.ComponentProperty.AsNoTracking().FirstAsync(d => d.Id == id);
                if (data != null && !data.IsGeneric) { defaultContext.ComponentProperty.Remove(data); await defaultContext.SaveChangesAsync(); }
                else { apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.Message = data.IsGeneric ? $"Generic deletion is not possible" : ""; }
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), id); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
    }
    public class StructureProcess : GlobalVariables
    {
        public async Task<ApiResponse> Get(int id)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                Dictionary<string, object> filter = []; if (id != 0) { filter.Add(nameof(id), id); }
                using DefaultContext defaultContext = new(GetConnection());
                List<Structure> listData = await defaultContext.Structure.AsNoTracking()
                                                .Include(d => d.ComponentStructure)
                                                .Include(d => d.Properties)
                                                    .ThenInclude(d => d.ComponentStructureProperty)
                                                        .ThenInclude(d => d.ComponentProperty)
                                                .OrFilter(filter).OrderBy(d => d.ShortIndex)
                                                .ToListAsync();
                if (listData.Count > 0)
                {
                    List<StructureSubStructure> structureMapping = await defaultContext.StructureSubStructure.AsNoTracking().ToListAsync();
                    List<StructurePropertyToStructure> structurePropertyToStoreMapping = await defaultContext.StructurePropertyToStructure.AsNoTracking().ToListAsync();

                    listData.ForEach(d =>
                    {
                        d.Properties.ForEach(p => p.ChildMapping = structurePropertyToStoreMapping.Where(m => m.StructurePropertyId == p.Id).Select(c => c.ChildStructureId).ToList());
                        d.ParentMapping = structureMapping.Where(c => c.ChildStructureId == d.Id).Select(c => c.ParentStructureId).ToList();
                        d.ChildMapping = structureMapping.Where(s => s.ParentStructureId == d.Id).Select(d => d.ChildStructureId).ToList();
                    });
                }
                apiResponse.Data = listData;
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), null); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
        public async Task<ApiResponse> Save(Structure data)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using DefaultContext defaultContext = new(GetConnection());
                using IDbContextTransaction transaction = defaultContext.Database.BeginTransaction();

                List<ComponentStructureProperty> properties = await defaultContext.ComponentStructureProperty.AsNoTracking().Where(d => d.ComponentStructureId == data.ComponentStructureId).ToListAsync();
                if (data.Id == 0 && !await defaultContext.Structure.AsNoTracking().AnyAsync(d => d.Identifier == data.Identifier))
                {
                    data.Properties ??= [];
                    data.Properties.AddRange(properties.Select(d => new StructureProperty { ComponentStructurePropertyId = d.Id, Type = nameof(String) }));
                    await defaultContext.Structure.AddAsync(data);
                }
                else if (data.Id != 0 && !await defaultContext.Structure.AsNoTracking().AnyAsync(d => d.Identifier == data.Identifier && d.Id != data.Id))
                {
                    List<StructureProperty> existingProperties = await defaultContext.StructureProperty.AsNoTracking().Where(d => d.StructureId == data.Id).ToListAsync();
                    data.Properties.AddRange(properties.Where(d => !existingProperties.Any(e => e.ComponentStructurePropertyId == d.Id)).ToList().Select(d => new StructureProperty { ComponentStructurePropertyId = d.Id, Type = nameof(String) }));

                    defaultContext.Structure.Update(data);
                }
                else { apiResponse.Status = (byte)StatusFlags.AlreadyExists; }
                await defaultContext.SaveChangesAsync();

                if (data.ParentStructureId != 0)
                {
                    defaultContext.StructureSubStructure.Add(new() { Id = 0, ChildStructureId = data.Id, ParentStructureId = data.ParentStructureId });
                    await defaultContext.SaveChangesAsync();
                }
                transaction.Commit();
                apiResponse.Data = data;
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), data); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
        public async Task<ApiResponse> Delete(int id)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using DefaultContext defaultContext = new(GetConnection());
                Structure data = await defaultContext.Structure.AsNoTracking().FirstAsync(d => d.Id == id);
                if (data != null)
                {
                    defaultContext.StructureProperty.RemoveRange(await defaultContext.StructureProperty.AsNoTracking().Where(d => d.StructureId == id).ToListAsync());
                    defaultContext.StructureSubStructure.RemoveRange(await defaultContext.StructureSubStructure.AsNoTracking().Where(d => d.ParentStructureId == id).ToListAsync());
                    defaultContext.Structure.Remove(data);
                    await defaultContext.SaveChangesAsync();
                }
                else { apiResponse.Status = (byte)StatusFlags.Failed; }
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), id); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
        public async Task<ApiResponse> ReIndex(List<ReindexStruct> data)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using DefaultContext defaultContext = new(GetConnection());
                List<Structure> structureList = await defaultContext.Structure.Where(d => data.Select(s => s.Id).Contains(d.Id)).ToListAsync();
                structureList.ForEach(structure => { structure.ShortIndex = data.First(d => d.Id == structure.Id).ShortIndex; });
                defaultContext.Structure.UpdateRange(structureList);
                await defaultContext.SaveChangesAsync();
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), data); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
    }
    public class StructurePropertyProcess : GlobalVariables
    {
        public async Task<ApiResponse> Get(int id)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                List<StructureProperty> listData = await GetDataList<StructureProperty>(id);
                using DefaultContext defaultContext = new(GetConnection());
                if (listData.Count > 0)
                {
                    List<StructurePropertyToStructure> structurePropertyToStoreMapping = await GetDataList<StructurePropertyToStructure>();
                    listData.ForEach(d => d.ChildMapping = structurePropertyToStoreMapping.Where(m => m.StructurePropertyId == d.Id).Select(c => c.ChildStructureId).ToList());
                }
                apiResponse.Data = listData;
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), null); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
        public async Task<ApiResponse> Save(StructureProperty data)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using DefaultContext defaultContext = new(GetConnection());
                using IDbContextTransaction transaction = defaultContext.Database.BeginTransaction();

                if (data.Id == 0 && !await defaultContext.StructureProperty.AsNoTracking().AnyAsync(d => d.ComponentStructurePropertyId == data.ComponentStructurePropertyId)) { await defaultContext.StructureProperty.AddAsync(data); }
                else if (data.Id != 0) { defaultContext.StructureProperty.Update(data); }
                else { apiResponse.Status = (byte)StatusFlags.AlreadyExists; }
                await defaultContext.SaveChangesAsync();

                if (data.ChildMapping != null)
                {
                    defaultContext.StructurePropertyToStructure.RemoveRange(await defaultContext.StructurePropertyToStructure.AsNoTracking().Where(d => d.StructurePropertyId == data.Id).ToListAsync());
                    defaultContext.StructurePropertyToStructure.AddRange(data.ChildMapping.Select(d => new StructurePropertyToStructure { StructurePropertyId = data.Id, ChildStructureId = d }));
                }
                await defaultContext.SaveChangesAsync();
                transaction.Commit();
                apiResponse.Data = data;
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), data); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
        public async Task<ApiResponse> Delete(int id)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using DefaultContext defaultContext = new(GetConnection());
                StructureProperty data = await defaultContext.StructureProperty.AsNoTracking().FirstAsync(d => d.Id == id);
                if (data != null) { defaultContext.StructureProperty.Remove(data); await defaultContext.SaveChangesAsync(); }
                else { apiResponse.Status = (byte)StatusFlags.Failed; }
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), id); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
    }
}