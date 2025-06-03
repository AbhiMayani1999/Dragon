using Dragon.Data;
using Dragon.Enm;
using Dragon.Model;
using Dragon.Model.Configs;
using Dragon.Provider;
using Microsoft.EntityFrameworkCore;

namespace Dragon.Business.Process.Configs
{
    public class KeyGroupProcess : GlobalVariables
    {
        public async Task<ApiResponse> Save(KeyGroup data)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using DefaultContext defaultContext = new(GetConnection());

                if (data.Id == 0 && !await defaultContext.KeyGroup.AsNoTracking().AnyAsync(d => d.Name == data.Name)) { await defaultContext.KeyGroup.AddAsync(data); }
                else if (data.Id != 0 && !await defaultContext.KeyGroup.AsNoTracking().AnyAsync(d => d.Name == data.Name && d.Id != data.Id)) { defaultContext.KeyGroup.Update(data); }
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
                if (!await defaultContext.KeyStore.AnyAsync(d => d.KeyGroupId == id))
                {
                    defaultContext.KeyGroup.Remove(await defaultContext.KeyGroup.AsNoTracking().FirstAsync(d => d.Id == id));
                    await defaultContext.SaveChangesAsync();
                }
                else { apiResponse.Status = (byte)StatusFlags.DependencyExists; }
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), id); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
    }
    public class KeyStoreProcess : GlobalVariables
    {
        public async Task<ApiResponse> Save(KeyStore data)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using DefaultContext defaultContext = new(GetConnection());

                if (data.Id == 0 && !await defaultContext.KeyStore.AsNoTracking().AnyAsync(d => d.Key == data.Key && d.KeyGroupId == data.KeyGroupId)) { await defaultContext.KeyStore.AddAsync(data); }
                else if (data.Id != 0 && !await defaultContext.KeyStore.AsNoTracking().AnyAsync(d => d.Key == data.Key && d.KeyGroupId == data.KeyGroupId && d.Id != data.Id)) { defaultContext.KeyStore.Update(data); }
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
                defaultContext.KeyStore.Remove(await defaultContext.KeyStore.AsNoTracking().FirstAsync(d => d.Id == id));
                await defaultContext.SaveChangesAsync();
                //if (!await defaultContext.KeyStore.AnyAsync(d => d.KeyStoreId == id))
                //{
                //}
                //else { apiResponse.Status = (byte)StatusFlags.DependencyExists; }
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), id); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
    }
}
