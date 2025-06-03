using Dragon.Data;
using Dragon.Enm;
using Dragon.Model;
using Dragon.Provider;
using Microsoft.EntityFrameworkCore;

namespace Dragon.Business.Process.Configs
{
    public class EmailConfigProcess : GlobalVariables
    {
        public async Task<ApiResponse> Save(EmailConfig data)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using DefaultContext defaultContext = new(GetConnection());

                if (data.Id == 0 && !await defaultContext.EmailConfig.AsNoTracking().AnyAsync(d => d.SenderEmail == data.SenderEmail)) { await defaultContext.EmailConfig.AddAsync(data); }
                else if (data.Id != 0 && !await defaultContext.EmailConfig.AsNoTracking().AnyAsync(d => d.SenderEmail == data.SenderEmail && d.Id != data.Id)) { defaultContext.EmailConfig.Update(data); }
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
                EmailConfig data = await defaultContext.EmailConfig.AsNoTracking().FirstAsync(d => d.Id == id);
                if (data != null) { defaultContext.EmailConfig.Remove(data); await defaultContext.SaveChangesAsync(); }
                else { apiResponse.Status = (byte)StatusFlags.Failed; }
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), id); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
    }
}
