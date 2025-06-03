using Dragon.Data;
using Dragon.Enm;
using Dragon.Model;
using Dragon.Model.Configs;
using Dragon.Provider;
using Microsoft.EntityFrameworkCore;

namespace Dragon.Business.Process.Configs
{
    public class UserProcess : GlobalVariables
    {
        public async Task<ApiResponse> Save(User data)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using DefaultContext defaultContext = new(GetConnection()); data = UpdateTransection(data);
                if (data.Id == 0 && !await defaultContext.User.AsNoTracking().AnyAsync(d => d.Username == data.Username)) { data.Password = EncryptionProvider.Encrypt(data.Password); _ = await defaultContext.User.AddAsync(data); }
                else if (data.Id != 0 && !await defaultContext.User.AsNoTracking().AnyAsync(d => d.Username == data.Username && d.Id != data.Id))
                {
                    data.IsUniversal = (await defaultContext.User.AsNoTracking().FirstOrDefaultAsync(d => d.Id == data.Id)).IsUniversal;
                    if (!IsOldPassword(data)) { data.Password = EncryptionProvider.Encrypt(data.Password); }
                    defaultContext.User.Update(data);
                }
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
                User data = await defaultContext.User.AsNoTracking().FirstAsync(d => d.Id == id);
                if (data != null && !data.IsUniversal) { defaultContext.User.Remove(data); await defaultContext.SaveChangesAsync(); }
                else { apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.Message = data.IsUniversal ? $"Universal user deletion is not possible" : ""; }
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), id); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
        private bool IsOldPassword(User data)
        {
            bool isSuccess = false;
            try
            {
                using DefaultContext defaultContext = new(GetConnection());
                User user = defaultContext.User.AsNoTracking().FirstOrDefault(d => d.Username == data.Username || d.Id == data.Id);
                isSuccess = FunctionProvider.IsEqualString(user.Password, data.Password);
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), data); throw; }
            return isSuccess;
        }
    }

    public class UserTypeProcess : GlobalVariables
    {
        public async Task<ApiResponse> Save(UserType data)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using DefaultContext defaultContext = new(GetConnection()); data = UpdateTransection(data);

                if (data.Id == 0 && !await defaultContext.UserType.AsNoTracking().AnyAsync(d => d.Name == data.Name)) { await defaultContext.UserType.AddAsync(data); }
                else if (data.Id != 0 && !await defaultContext.UserType.AsNoTracking().AnyAsync(d => d.Name == data.Name && d.Id != data.Id)) { defaultContext.UserType.Update(data); }
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
                UserType data = await defaultContext.UserType.AsNoTracking().FirstAsync(d => d.Id == id);
                if (data != null && !await defaultContext.User.AsNoTracking().AnyAsync(d => d.UserTypeId == id)) { defaultContext.UserType.Remove(data); await defaultContext.SaveChangesAsync(); }
                else { apiResponse.Status = (byte)StatusFlags.DependencyExists; }
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), id); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
    }

    public class UserNavigationProcess : GlobalVariables
    {
        public async Task<ApiResponse> Get(int id)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                Dictionary<string, object> filter = []; if (id != 0) { filter.Add(nameof(id), id); }
                using DefaultContext defaultContext = new(GetConnection());
                List<User> userList = await defaultContext.User.AsNoTracking().OrFilter(filter).ToListAsync();
                List<UserNavigation> userNavigationList = await defaultContext.UserNavigation.AsNoTracking().OrFilter(filter).ToListAsync();
                userList.ForEach(user => { user.Password = ""; user.IsUniversal = false; user.UserTypeId = 0; user.Navigation = userNavigationList.Where(d => d.UserId == user.Id).ToList(); });
                apiResponse.Data = userList;
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), null); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
        public async Task<ApiResponse> Save(List<UserNavigation> data)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using DefaultContext defaultContext = new(GetConnection());
                if (data.Count != 0)
                {
                    defaultContext.UserNavigation.RemoveRange(defaultContext.UserNavigation.AsNoTracking().Where(d => d.UserId == data.First().UserId));
                    await defaultContext.UserNavigation.AddRangeAsync(data);
                }
                else { apiResponse.Status = (byte)StatusFlags.Failed; }
                await defaultContext.SaveChangesAsync();
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), data); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
    }
    public class UserTypeNavigationProcess : GlobalVariables
    {
        public async Task<ApiResponse> Get(int id)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                Dictionary<string, object> filter = []; if (id != 0) { filter.Add(nameof(id), id); }
                using DefaultContext defaultContext = new(GetConnection());
                List<UserType> userTypeList = await defaultContext.UserType.AsNoTracking().OrFilter(filter).ToListAsync();
                List<UserTypeNavigation> userTypeNavigationList = await defaultContext.UserTypeNavigation.AsNoTracking().OrFilter(filter).ToListAsync();
                userTypeList.ForEach(userType => { userType.IsActive = false; userType.IsAdmin = false; userType.Navigation = userTypeNavigationList.Where(d => d.UserTypeId == userType.Id).ToList(); });
                apiResponse.Data = userTypeList;
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), null); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
        public async Task<ApiResponse> Save(List<UserTypeNavigation> data)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using DefaultContext defaultContext = new(GetConnection());
                if (data.Count != 0)
                {
                    defaultContext.UserTypeNavigation.RemoveRange(defaultContext.UserTypeNavigation.AsNoTracking().Where(d => d.UserTypeId == data.First().UserTypeId));
                    await defaultContext.UserTypeNavigation.AddRangeAsync(data);
                }
                else { apiResponse.Status = (byte)StatusFlags.Failed; }
                await defaultContext.SaveChangesAsync();
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), data); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
    }
}
