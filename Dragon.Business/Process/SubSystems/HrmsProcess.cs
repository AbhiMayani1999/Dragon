using Dragon.Data;
using Dragon.Enm;
using Dragon.Model;
using Dragon.Model.SubSystems;
using Dragon.Provider;
using Microsoft.EntityFrameworkCore;

namespace Dragon.Business.Process.SubSystems
{
    public class HrmsCompanyProcess : GlobalVariables
    {
        public async Task<ApiResponse> Save(HrmsCompany data)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using HrmsContext hrmsContext = new(GetConnection());

                if (data.Id == 0 && !await hrmsContext.HrmsCompany.AsNoTracking().AnyAsync(d => d.Name == data.Name)) { await hrmsContext.HrmsCompany.AddAsync(data); }
                else if (data.Id != 0 && !await hrmsContext.HrmsCompany.AsNoTracking().AnyAsync(d => d.Name == data.Name && d.Id != data.Id)) { hrmsContext.HrmsCompany.Update(data); }
                else { apiResponse.Status = (byte)StatusFlags.AlreadyExists; }
                await hrmsContext.SaveChangesAsync();
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), data); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
        public async Task<ApiResponse> Delete(int id)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using HrmsContext hrmsContext = new(GetConnection());
                if (!await hrmsContext.HrmsCompanyDepartment.AnyAsync(d => d.CompanyId == id) &&
                    !await hrmsContext.HrmsCompanyBank.AnyAsync(d => d.CompanyId == id) &&
                    !await hrmsContext.HrmsCompanyInvoice.AnyAsync(d => d.FromCompanyId == id) &&
                    !await hrmsContext.HrmsCompanyInvoice.AnyAsync(d => d.ToCompanyId == id)
                    )
                {
                    hrmsContext.HrmsCompany.Remove(await hrmsContext.HrmsCompany.AsNoTracking().FirstAsync(d => d.Id == id));
                    await hrmsContext.SaveChangesAsync();
                }
                else { apiResponse.Status = (byte)StatusFlags.DependencyExists; }
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), id); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
    }

    public class HrmsCompanyBankProcess : GlobalVariables
    {
        public async Task<ApiResponse> Save(HrmsCompanyBank data)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using HrmsContext hrmsContext = new(GetConnection());

                if (data.Id == 0 && !await hrmsContext.HrmsCompanyBank.AsNoTracking().AnyAsync(d => d.CompanyId == data.CompanyId && d.BankName == data.BankName && d.Name == data.Name)) { await hrmsContext.HrmsCompanyBank.AddAsync(data); }
                else if (data.Id != 0 && !await hrmsContext.HrmsCompanyBank.AsNoTracking().AnyAsync(d => d.CompanyId == data.CompanyId && d.BankName == data.BankName && d.Name == data.Name && d.Id != data.Id)) { hrmsContext.HrmsCompanyBank.Update(data); }
                else { apiResponse.Status = (byte)StatusFlags.AlreadyExists; }
                await hrmsContext.SaveChangesAsync();
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), data); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
        public async Task<ApiResponse> Delete(int id)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using HrmsContext hrmsContext = new(GetConnection());
                hrmsContext.HrmsCompanyBank.Remove(await hrmsContext.HrmsCompanyBank.AsNoTracking().FirstAsync(d => d.Id == id));
                await hrmsContext.SaveChangesAsync();
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), id); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
    }

    public class HrmsCompanyDepartmentProcess : GlobalVariables
    {
        public async Task<ApiResponse> Save(HrmsCompanyDepartment data)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using HrmsContext hrmsContext = new(GetConnection());

                if (data.Id == 0 && !await hrmsContext.HrmsCompanyDepartment.AsNoTracking().AnyAsync(d => d.CompanyId == data.CompanyId && d.Name == data.Name)) { await hrmsContext.HrmsCompanyDepartment.AddAsync(data); }
                else if (data.Id != 0 && !await hrmsContext.HrmsCompanyDepartment.AsNoTracking().AnyAsync(d => d.CompanyId == data.CompanyId && d.Name == data.Name && d.Id != data.Id)) { hrmsContext.HrmsCompanyDepartment.Update(data); }
                else { apiResponse.Status = (byte)StatusFlags.AlreadyExists; }
                await hrmsContext.SaveChangesAsync();
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), data); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
        public async Task<ApiResponse> Delete(int id)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using HrmsContext hrmsContext = new(GetConnection());
                if (!await hrmsContext.HrmsEmployee.AnyAsync(d => d.DepartmentId == id))
                { hrmsContext.HrmsCompanyDepartment.Remove(await hrmsContext.HrmsCompanyDepartment.AsNoTracking().FirstAsync(d => d.Id == id)); await hrmsContext.SaveChangesAsync(); }
                else { apiResponse.Status = (byte)StatusFlags.DependencyExists; }
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), id); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
    }

    public class HrmsEmployeeProcess : GlobalVariables
    {
        public async Task<ApiResponse> Save(HrmsEmployee data)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using HrmsContext hrmsContext = new(GetConnection());
                data = FileUpload(data, nameof(data.ProfileUrl), data.Id != 0 ? (await hrmsContext.HrmsEmployee.AsNoTracking().FirstOrDefaultAsync(d => d.Id == data.Id)).ProfileUrl : data.ProfileUrl, data.Code);
                if (data.Id == 0 && !await hrmsContext.HrmsEmployee.AsNoTracking().AnyAsync(d => d.Name == data.Name)) { await hrmsContext.HrmsEmployee.AddAsync(data); }
                else if (data.Id != 0 && !await hrmsContext.HrmsEmployee.AsNoTracking().AnyAsync(d => d.Name == data.Name && d.Id != data.Id)) { hrmsContext.HrmsEmployee.Update(data); }
                else { apiResponse.Status = (byte)StatusFlags.AlreadyExists; }
                await hrmsContext.SaveChangesAsync();
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
                using HrmsContext hrmsContext = new(GetConnection());
                if (!await hrmsContext.HrmsEmployeeSalary.AnyAsync(d => d.EmployeeId == id) &&
                    !await hrmsContext.HrmsEmployeeAppraisal.AnyAsync(d => d.EmployeeId == id) &&
                    !await hrmsContext.HrmsEmployeeDocument.AnyAsync(d => d.EmployeeId == id) &&
                    !await hrmsContext.HrmsEmployeePunching.AnyAsync(d => d.EmployeeId == id))
                {
                    hrmsContext.HrmsEmployee.Remove(await hrmsContext.HrmsEmployee.AsNoTracking().FirstAsync(d => d.Id == id));
                    await hrmsContext.SaveChangesAsync();
                }
                else { apiResponse.Status = (byte)StatusFlags.DependencyExists; }
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), id); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
    }

    public class HrmsEmployeeAppraisalProcess : GlobalVariables
    {
        public async Task<ApiResponse> Save(HrmsEmployeeAppraisal data)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using HrmsContext hrmsContext = new(GetConnection());

                if (data.Id == 0 && !await hrmsContext.HrmsEmployeeAppraisal.AsNoTracking().AnyAsync(d => d.EmployeeId == data.EmployeeId && d.AppraisalDate == data.AppraisalDate)) { await hrmsContext.HrmsEmployeeAppraisal.AddAsync(data); }
                else if (data.Id != 0 && !await hrmsContext.HrmsEmployeeAppraisal.AsNoTracking().AnyAsync(d => d.EmployeeId == data.EmployeeId && d.AppraisalDate == data.AppraisalDate && d.Id != data.Id)) { hrmsContext.HrmsEmployeeAppraisal.Update(data); }
                else { apiResponse.Status = (byte)StatusFlags.AlreadyExists; }
                await hrmsContext.SaveChangesAsync();
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), data); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
        public async Task<ApiResponse> Delete(int id)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using HrmsContext hrmsContext = new(GetConnection());
                hrmsContext.HrmsEmployeeAppraisal.Remove(await hrmsContext.HrmsEmployeeAppraisal.AsNoTracking().FirstAsync(d => d.Id == id));
                await hrmsContext.SaveChangesAsync();
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), id); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
    }

    public class HrmsEmployeeSalaryProcess : GlobalVariables
    {
        public async Task<ApiResponse> Save(HrmsEmployeeSalary data)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using HrmsContext hrmsContext = new(GetConnection());

                if (data.Id == 0 && !await hrmsContext.HrmsEmployeeSalary.AsNoTracking().AnyAsync(d => d.EmployeeId == data.EmployeeId && d.Date == data.Date)) { await hrmsContext.HrmsEmployeeSalary.AddAsync(data); }
                else if (data.Id != 0 && !await hrmsContext.HrmsEmployeeSalary.AsNoTracking().AnyAsync(d => d.EmployeeId == data.EmployeeId && d.Date == data.Date && d.Id != data.Id)) { hrmsContext.HrmsEmployeeSalary.Update(data); }
                else { apiResponse.Status = (byte)StatusFlags.AlreadyExists; }
                await hrmsContext.SaveChangesAsync();
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), data); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
        public async Task<ApiResponse> Delete(int id)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using HrmsContext hrmsContext = new(GetConnection());
                hrmsContext.HrmsEmployeeSalary.Remove(await hrmsContext.HrmsEmployeeSalary.AsNoTracking().FirstAsync(d => d.Id == id));
                await hrmsContext.SaveChangesAsync();
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), id); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
    }

    public class HrmsEmployeeDocumentProcess : GlobalVariables
    {
        public async Task<ApiResponse> Save(HrmsEmployeeDocument data)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using HrmsContext hrmsContext = new(GetConnection());

                if (data.Id == 0 && !await hrmsContext.HrmsEmployeeDocument.AsNoTracking().AnyAsync(d => d.EmployeeId == data.EmployeeId && d.DocumentName == data.DocumentName)) { await hrmsContext.HrmsEmployeeDocument.AddAsync(data); }
                else if (data.Id != 0 && !await hrmsContext.HrmsEmployeeDocument.AsNoTracking().AnyAsync(d => d.EmployeeId == data.EmployeeId && d.DocumentName == data.DocumentName && d.Id != data.Id)) { hrmsContext.HrmsEmployeeDocument.Update(data); }
                else if (data.Id != 0 && !await hrmsContext.HrmsEmployeeDocument.AsNoTracking().AnyAsync(d => d.EmployeeId == data.EmployeeId && d.DocumentName == data.DocumentName && d.Id != data.Id)) { hrmsContext.HrmsEmployeeDocument.Update(data); }
                else { apiResponse.Status = (byte)StatusFlags.AlreadyExists; }
                await hrmsContext.SaveChangesAsync();
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), data); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
        public async Task<ApiResponse> Delete(int id)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using HrmsContext hrmsContext = new(GetConnection());
                hrmsContext.HrmsEmployeeDocument.Remove(await hrmsContext.HrmsEmployeeDocument.AsNoTracking().FirstAsync(d => d.Id == id));
                await hrmsContext.SaveChangesAsync();
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), id); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
    }

    public class HrmsEmployeeBankProcess : GlobalVariables
    {
        public async Task<ApiResponse> Save(HrmsEmployeeBank data)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using HrmsContext hrmsContext = new(GetConnection());

                if (data.Id == 0 && !await hrmsContext.HrmsEmployeeBank.AsNoTracking().AnyAsync(d => d.EmployeeId == data.EmployeeId && d.BankName == data.BankName && d.Name == data.Name)) { await hrmsContext.HrmsEmployeeBank.AddAsync(data); }
                else if (data.Id != 0 && !await hrmsContext.HrmsEmployeeBank.AsNoTracking().AnyAsync(d => d.EmployeeId == data.EmployeeId && d.BankName == data.BankName && d.Name == data.Name && d.Id != data.Id)) { hrmsContext.HrmsEmployeeBank.Update(data); }
                else { apiResponse.Status = (byte)StatusFlags.AlreadyExists; }
                await hrmsContext.SaveChangesAsync();
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), data); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
        public async Task<ApiResponse> Delete(int id)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try
            {
                using HrmsContext hrmsContext = new(GetConnection());
                hrmsContext.HrmsEmployeeBank.Remove(await hrmsContext.HrmsEmployeeBank.AsNoTracking().FirstAsync(d => d.Id == id));
                await hrmsContext.SaveChangesAsync();
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), id); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
    }
}
