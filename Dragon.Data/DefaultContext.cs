using Dragon.Model.Configs;
using Dragon.Model.SubSystems;
using Dragon.Provider;
using Microsoft.EntityFrameworkCore;
using static Dragon.Provider.ConnectionProvider;

namespace Dragon.Data
{
    public class DefaultContext : ContextTables
    {
        public DefaultContext(Connection connection) : base() { CurrentConnection = connection; }
    }
    public class ContextTables : ContextProvider
    {
        public DbSet<User> User { get; set; }
        public DbSet<UserType> UserType { get; set; }

        public DbSet<Connection> Connection { get; set; }
        public DbSet<EmailConfig> EmailConfig { get; set; }
        public DbSet<DomainConnect> DomainConnect { get; set; }
        public DbSet<DomainSetting> DomainSetting { get; set; }

        public DbSet<KeyGroup> KeyGroup { get; set; }
        public DbSet<KeyStore> KeyStore { get; set; }

        public DbSet<Navigation> Navigation { get; set; }
        public DbSet<UserNavigation> UserNavigation { get; set; }
        public DbSet<UserTypeNavigation> UserTypeNavigation { get; set; }

        public DbSet<ComponentProperty> ComponentProperty { get; set; }
        public DbSet<ComponentStructure> ComponentStructure { get; set; }
        public DbSet<ComponentStructureProperty> ComponentStructureProperty { get; set; }
        public DbSet<ComponentStructureSubComponent> ComponentStructureSubComponent { get; set; }
        public DbSet<Structure> Structure { get; set; }
        public DbSet<StructureProperty> StructureProperty { get; set; }
        public DbSet<StructureSubStructure> StructureSubStructure { get; set; }
        public DbSet<StructurePropertyToStructure> StructurePropertyToStructure { get; set; }

        public DbSet<HrmsCompany> HrmsCompany { get; set; }
        public DbSet<HrmsCompanyBank> HrmsCompanyBank { get; set; }
        public DbSet<HrmsCompanyInvoice> HrmsCompanyInvoice { get; set; }
        public DbSet<HrmsCompanyDepartment> HrmsCompanyDepartment { get; set; }
        public DbSet<HrmsCompanyInvoiceService> HrmsCompanyInvoiceService { get; set; }
        public DbSet<HrmsEmployee> HrmsEmployee { get; set; }
        public DbSet<HrmsEmployeeBank> HrmsEmployeeBank { get; set; }
        public DbSet<HrmsEmployeeSalary> HrmsEmployeeSalary { get; set; }
        public DbSet<HrmsEmployeeDocument> HrmsEmployeeDocument { get; set; }
        public DbSet<HrmsEmployeeAppraisal> HrmsEmployeeAppraisal { get; set; }
        public DbSet<HrmsEmployeePunching> HrmsEmployeePunching { get; set; }
    }
    public class HrmsContext : ContextProvider
    {
        public HrmsContext(Connection connection) : base() { CurrentConnection = connection; }

        public DbSet<HrmsCompany> HrmsCompany { get; set; }
        public DbSet<HrmsCompanyBank> HrmsCompanyBank { get; set; }
        public DbSet<HrmsCompanyInvoice> HrmsCompanyInvoice { get; set; }
        public DbSet<HrmsCompanyDepartment> HrmsCompanyDepartment { get; set; }
        public DbSet<HrmsCompanyInvoiceService> HrmsCompanyInvoiceService { get; set; }

        public DbSet<HrmsEmployee> HrmsEmployee { get; set; }
        public DbSet<HrmsEmployeeBank> HrmsEmployeeBank { get; set; }
        public DbSet<HrmsEmployeeSalary> HrmsEmployeeSalary { get; set; }
        public DbSet<HrmsEmployeeDocument> HrmsEmployeeDocument { get; set; }
        public DbSet<HrmsEmployeeAppraisal> HrmsEmployeeAppraisal { get; set; }
        public DbSet<HrmsEmployeePunching> HrmsEmployeePunching { get; set; }
    }
}