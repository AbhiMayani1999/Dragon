using Dragon.Data;
using Dragon.Enm;
using Dragon.Model;
using Dragon.Model.Configs;
using Dragon.Provider;
using Microsoft.EntityFrameworkCore;
using static Dragon.Provider.ConnectionProvider;

namespace Dragon.Business.Process.Configs
{
    public class ConnectionProcess : GlobalVariables
    {
        public async Task<ApiResponse> Save(Connection data)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using DefaultContext defaultContext = new(GetConnection());

                if (data.Id == 0 && !await defaultContext.Connection.AsNoTracking().AnyAsync(d => d.TenantCode == data.TenantCode)) { await defaultContext.Connection.AddAsync(data); }
                else if (data.Id != 0 && !await defaultContext.Connection.AsNoTracking().AnyAsync(d => d.TenantCode == data.TenantCode && d.Id != data.Id)) { defaultContext.Connection.Update(data); }
                else { apiResponse.Status = (byte)StatusFlags.AlreadyExists; }
                await defaultContext.SaveChangesAsync(); ConnectionProvider.Provider.ReloadAllConnection();
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
                Connection data = await defaultContext.Connection.AsNoTracking().FirstAsync(d => d.Id == id);
                if (data != null) { data.IsDeleted = true; defaultContext.Connection.Update(data); await defaultContext.SaveChangesAsync(); ConnectionProvider.Provider.ReloadAllConnection(); }
                else { apiResponse.Status = (byte)StatusFlags.Failed; }
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), id); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
    }

    public class DomainConnectProcess : GlobalVariables
    {
        public async Task<ApiResponse> Save(DomainConnect data)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using DefaultContext defaultContext = new(GetConnection());

                if (data.Id == 0 && !await defaultContext.DomainConnect.AsNoTracking().AnyAsync(d => d.TenantCode == data.TenantCode)) { await defaultContext.DomainConnect.AddAsync(data); }
                else if (data.Id != 0 && !await defaultContext.DomainConnect.AsNoTracking().AnyAsync(d => d.TenantCode == data.TenantCode && d.Id != data.Id)) { defaultContext.DomainConnect.Update(data); }
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
                if (!await defaultContext.DomainSetting.AnyAsync(d => d.DomainConnectId == id))
                {
                    defaultContext.DomainConnect.Remove(await defaultContext.DomainConnect.AsNoTracking().FirstAsync(d => d.Id == id));
                    await defaultContext.SaveChangesAsync();
                }
                else { apiResponse.Status = (byte)StatusFlags.DependencyExists; }
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), id); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
    }

    public class DomainSettingProcess : GlobalVariables
    {
        public async Task<ApiResponse> Save(DomainSetting data)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using DefaultContext defaultContext = new(GetConnection());

                if (data.Id == 0 && !await defaultContext.DomainSetting.AsNoTracking().AnyAsync(d => d.BrandName == data.BrandName)) { await defaultContext.DomainSetting.AddAsync(data); }
                else if (data.Id != 0 && !await defaultContext.DomainSetting.AsNoTracking().AnyAsync(d => d.BrandName == data.BrandName && d.Id != data.Id)) { defaultContext.DomainSetting.Update(data); }
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
                DomainSetting data = await defaultContext.DomainSetting.AsNoTracking().FirstAsync(d => d.Id == id);
                if (data != null) { defaultContext.DomainSetting.Remove(data); await defaultContext.SaveChangesAsync(); }
                else { apiResponse.Status = (byte)StatusFlags.Failed; }
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), id); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
    }
}