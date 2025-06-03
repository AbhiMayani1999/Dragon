using Dragon.Data;
using Dragon.Enm;
using Dragon.Model;
using Dragon.Model.Configs;
using Dragon.Provider;
using Microsoft.EntityFrameworkCore;

namespace Dragon.Business.Process.Configs
{
    public class NavigationProcess : GlobalVariables
    {
        public async Task<ApiResponse> Save(Navigation data)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using DefaultContext defaultContext = new(GetConnection());

                if (data.Id == 0 && !await defaultContext.Navigation.AsNoTracking().AnyAsync(d => d.Code == data.Code)) { await defaultContext.Navigation.AddAsync(data); }
                else if (data.Id != 0 && !await defaultContext.Navigation.AsNoTracking().AnyAsync(d => d.Code == data.Code && d.Id != data.Id)) { defaultContext.Navigation.Update(data); }
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
                Navigation data = await defaultContext.Navigation.AsNoTracking().FirstAsync(d => d.Id == id);
                if (data != null && !await defaultContext.Navigation.AsNoTracking().AnyAsync(d => d.ParentNavigationId == id)) { defaultContext.Navigation.Remove(data); await defaultContext.SaveChangesAsync(); }
                else { apiResponse.Status = (byte)StatusFlags.DependencyExists; }
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), id); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
        public async Task<ApiResponse> GetUserPermittedNavigation()
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Failed };
            try
            {
                using DefaultContext defaultContext = new(GetConnection());
                List<Navigation> navigationList = await defaultContext.Navigation.Where(d => d.IsActive).OrderBy(n => n.SortIndex).ToListAsync();
                if (!CurrentUser.IsUniversal)
                {
                    List<UserNavigation> userNavigation = await defaultContext.UserNavigation.Where(d => d.UserId == CurrentUser.Id).ToListAsync();
                    List<UserTypeNavigation> userTypeNavigation = await defaultContext.UserTypeNavigation.Where(d => d.UserTypeId == CurrentUser.UserTypeId).ToListAsync();
                    navigationList.ForEach(item =>
                    {
                        UserNavigation userNav = userNavigation.FirstOrDefault(d => d.Code == item.Code);
                        if (userNav != null) { item.IsCreate = userNav.IsCreate; item.IsView = userNav.IsView; item.IsEdit = userNav.IsEdit; item.IsDelete = userNav.IsDelete; }

                        if (userNav != null && userTypeNavigation.Any(d => d.Code == item.Code))
                        {
                            UserTypeNavigation userTypeNav = userTypeNavigation.FirstOrDefault(d => d.Code == item.Code);
                            item.IsCreate = userTypeNav.IsCreate; item.IsView = userTypeNav.IsView; item.IsEdit = userTypeNav.IsEdit; item.IsDelete = userTypeNav.IsDelete;
                        }
                    });

                    navigationList.RemoveAll(d => !userNavigation.Any(un => un.Code == d.Code) && !userTypeNavigation.Any(utn => utn.Code == d.Code));
                    navigationList.RemoveAll(d => !d.IsView && !d.IsCreate && !d.IsEdit && !d.IsDelete);

                    if (navigationList.Count == 0) { apiResponse.Message = "No permission found"; } else { apiResponse.Status = (byte)StatusFlags.Success; }
                }
                else { navigationList.ForEach((d) => { d.IsCreate = true; d.IsView = true; d.IsEdit = true; d.IsDelete = true; }); apiResponse.Status = (byte)StatusFlags.Success; }
                apiResponse.Data = navigationList;
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), null); apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
    }
}

//dataList.ForEach(MainNav => MainNav.ChildItems.RemoveAll(d => !d.IsActive));
// dataList.RemoveAll(d => d.ParentNavigationId != null);
//d.ChildItems.ForEach(c => { c.IsCreate = true; c.IsView = true; c.IsEdit = true; c.IsDelete = true; });
//d.ChildItems = d.ChildItems.OrderBy(n => n.SortIndex).ToList();