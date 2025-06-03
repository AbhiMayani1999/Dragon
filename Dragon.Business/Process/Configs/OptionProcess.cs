using Dragon.Data;
using Dragon.Enm;
using Dragon.Model;
using Dragon.Model.Configs;
using Dragon.Provider;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Dragon.Business.Process.Configs
{
    public class OptionProcess : GlobalVariables
    {
        public ApiResponse FillMultipleOptions(List<OptionsTransfer> propertyOptions)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            List<string> failedList = [];
            Parallel.ForEach(propertyOptions, options =>
            {
                try { options.Options = !string.IsNullOrWhiteSpace(options.KeyStore) ? FillKeyStoreOptions(options).Result : FillTableOptions(options).Result; }
                catch (Exception ex) { failedList.Add(!string.IsNullOrWhiteSpace(options.KeyStore) ? options.KeyStore : options.Table); apiResponse.DetailedError = $"{Convert.ToString(ex)} \n"; LogsProvider.WriteErrorLog(Convert.ToString(ex), propertyOptions); apiResponse.Status = (byte)StatusFlags.Failed; }
            });
            if (failedList != null && failedList.Count > 0) { apiResponse.Message = $"{failedList.ToCommaSeparateString()} Failed to Load"; }
            apiResponse.Data = propertyOptions;
            return apiResponse;
        }
        public async Task<List<Options>> FillKeyStoreOptions(OptionsTransfer optionTransfer)
        {
            List<Options> DataOptions = [];
            if (!string.IsNullOrWhiteSpace(optionTransfer.KeyStore))
            {
                using DefaultContext defaultContext = new(GetConnection());
                KeyGroup storeGroup = await defaultContext.KeyGroup.AsNoTracking().Include(d => d.KeyStore).FirstOrDefaultAsync(d => d.Name == optionTransfer.KeyStore);
                if (storeGroup != null && storeGroup.KeyStore != null && storeGroup.KeyStore.Count > 0) { DataOptions = storeGroup.KeyStore.Select(d => new Options { Key = d.Key, Value = d.Value }).ToList(); }
            }
            return DataOptions;
        }
        private async Task<List<Options>> FillTableOptions(OptionsTransfer optionTransfer)
        {
            List<Options> DataOptions = [];
            if (!string.IsNullOrWhiteSpace(optionTransfer.Table) && !string.IsNullOrWhiteSpace(optionTransfer.Key) && !string.IsNullOrWhiteSpace(optionTransfer.Value))
            {
                using DefaultContext defaultContext = new(GetConnection());
                if (defaultContext.GetType().GetProperty(optionTransfer.Table) != null)
                {
                    List<object> table = await (defaultContext.GetType().GetProperty(optionTransfer.Table).GetValue(defaultContext, null) as IQueryable<object>).ToListAsync();
                    if (table.Count > 0)
                    {
                        Type type = table.FirstOrDefault().GetType();
                        PropertyInfo KeyField = type.GetProperty(optionTransfer.Key);
                        PropertyInfo ValueField = type.GetProperty(optionTransfer.Value);
                        if (string.IsNullOrWhiteSpace(optionTransfer.CascadeBy))
                        { DataOptions = table.Select(x => new Options() { Key = KeyField.GetValue(x), Value = Convert.ToString(ValueField.GetValue(x)) }).ToList(); }
                        else
                        {
                            PropertyInfo CascadeField = type.GetProperty(optionTransfer.CascadeBy);
                            DataOptions = table.Where(d => Convert.ToString(CascadeField.GetValue(d)) == optionTransfer.CascadeValue)
                                .Select(x => new Options() { Key = KeyField.GetValue(x), Value = Convert.ToString(ValueField.GetValue(x)) }).ToList();
                        }
                    }
                }
            }
            return DataOptions;
        }
    }
}