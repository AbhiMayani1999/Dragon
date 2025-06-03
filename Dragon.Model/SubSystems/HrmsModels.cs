using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Dragon.Model.SubSystems
{
    public abstract class HrmsEmployeeSubPart
    {
        [Key] public int Id { get; set; }
        [Required] public int EmployeeId { get; set; }
        [JsonIgnore][ForeignKey(nameof(EmployeeId))] public HrmsEmployee Employee { get; set; }
    }
    public abstract class HrmsBank
    {
        [Key] public int Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string BankName { get; set; }
        [Required] public string IfscCode { get; set; }
        [Required] public string AccountNumber { get; set; }
        [Required] public string AccountType { get; set; }
        public string SwiftCode { get; set; }
        public string Iban { get; set; }
    }
    public abstract class HrmsContactDetails
    {
        [Required] public string Name { get; set; }
        [Required] public string AddressLine1 { get; set; }
        [Required] public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Pincode { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Pancard { get; set; }
    }
    public struct HrmsPunchingStatus
    {
        public DateTime InTime { get; set; }
        public bool PunchInComplete { get; set; }
        public bool PunchOutComplete { get; set; }
        public bool BreakComplete { get; set; }
        public bool BreakStarted { get; set; }
    }

    [Table(nameof(HrmsCompany))]
    public class HrmsCompany : HrmsContactDetails
    {
        [Key] public int Id { get; set; }
        [Required] public string Proprietor { get; set; }
        [Required] public bool IsSelf { get; set; } = false;
        public string GstNo { get; set; }
        public string LutArn { get; set; }
        public string GroupCode { get; set; }

        public List<HrmsCompanyBank> Bank { get; set; }
    }

    [Table(nameof(HrmsCompanyBank))]
    public class HrmsCompanyBank : HrmsBank
    {
        [Required] public int CompanyId { get; set; }
        [JsonIgnore][ForeignKey(nameof(CompanyId))] public HrmsCompany Company { get; set; }
    }

    [Table(nameof(HrmsCompanyInvoice))]
    public class HrmsCompanyInvoice
    {
        [Key] public int Id { get; set; }
        [Required] public string InvoiceNumber { get; set; }
        [Required] public bool SameStateInvoice { get; set; }
        [Required] public bool InternationalInvoice { get; set; }
        [Required] public DateTime InvoiceDate { get; set; } = DateTime.Now;

        [Required] public float Total { get; set; }
        [Required] public float TaxAmount { get; set; }
        public string TotalInWords { get; set; }
        public float TaxPercentage { get; set; }
        public float SgstAmount { get; set; }
        public float CgstAmount { get; set; }
        public float SgstPercentage { get; set; }
        public float CgstPercentage { get; set; }
        public string PaymentTerms { get; set; }

        public List<HrmsCompanyInvoiceService> Services { get; set; }

        public int? FromCompanyId { get; set; }
        [ForeignKey(nameof(FromCompanyId))] public HrmsCompany FromCompany { get; set; }
        public int? ToCompanyId { get; set; }
        [ForeignKey(nameof(ToCompanyId))] public HrmsCompany ToCompany { get; set; }

        //[NotMapped] public string ViewDate => InvoiceDate.ToString("dd-MM-yyyy");

        //[NotMapped] public string PrintTotal => Total.ToString("#,#", CultureInfo.CreateSpecificCulture("hi-IN"));
        //[NotMapped] public string PrintTaxAmount => TaxAmount.ToString("#,#", CultureInfo.CreateSpecificCulture("hi-IN"));
        //[NotMapped] public string PrintSgstAmount => SgstAmount.ToString("#,#", CultureInfo.CreateSpecificCulture("hi-IN"));
        //[NotMapped] public string PrintCgstAmount => CgstAmount.ToString("#,#", CultureInfo.CreateSpecificCulture("hi-IN"));
    }

    [Table(nameof(HrmsCompanyDepartment))]
    public class HrmsCompanyDepartment
    {
        [Key] public int Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public bool IsActive { get; set; } = true;
        [NotMapped] public string DepartmentHead { get; set; }

        public int? CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))] public HrmsCompany Company { get; set; }

        public List<HrmsEmployee> Employees { get; set; }
        [NotMapped] public int EmployeeCount => Employees != null ? Employees.Count : 0;
    }

    [Table(nameof(HrmsCompanyInvoiceService))]
    public class HrmsCompanyInvoiceService
    {
        [Key] public int Id { get; set; }
        [Required] public string Description { get; set; }
        [Required] public float Amount { get; set; }
        [Required] public float RatePerMonth { get; set; }
        [Required] public int InvoiceId { get; set; }
        [JsonIgnore][ForeignKey(nameof(InvoiceId))] public HrmsCompanyInvoice Invoice { get; set; }

        [NotMapped] public string PrintAmount => Amount.ToString("#,#", CultureInfo.CreateSpecificCulture("hi-IN"));
        [NotMapped] public string PrintRatePerMonth => RatePerMonth.ToString("#,#", CultureInfo.CreateSpecificCulture("hi-IN"));
    }


    [Table(nameof(HrmsEmployee))]
    public class HrmsEmployee : HrmsContactDetails
    {
        [Key] public int Id { get; set; }
        [Required] public string Salutation { get; set; }
        [Required] public string ShortName { get; set; }
        [Required] public string Code { get; set; }
        [Required] public string Designation { get; set; }
        [Required] public int Ctc { get; set; }
        [Required] public DateTime JoiningDate { get; set; }
        [Required] public DateTime DateOfBirth { get; set; }
        [Required] public bool IsFullTime { get; set; } = true;
        [Required] public bool IsGenerateDocuments { get; set; } = false;
        public DateTime? ConfirmationDate { get; set; }
        public DateTime? RelievingDate { get; set; }
        public string ProfileUrl { get; set; }

        public int? DepartmentId { get; set; }
        [JsonIgnore][ForeignKey(nameof(DepartmentId))] public HrmsCompanyDepartment Department { get; set; }

        public List<HrmsEmployeeDocument> Documents { get; set; }
        public List<HrmsEmployeeAppraisal> Appraisals { get; set; }
        public List<HrmsEmployeeBank> Banks { get; set; }
        public List<HrmsEmployeeSalary> Salaries { get; set; }

        [NotMapped] public bool IsActive => RelievingDate == null;
        [NotMapped] public string DepartmentName => Department != null ? Department.Name : "";
        [NotMapped] public HrmsPunchingStatus PunchingStatus { get; set; }
        [NotMapped] public HrmsEmployeeSalary PrintSalary { get; set; }
        [NotMapped] public HrmsEmployeeBank PrintBank { get; set; }
    }

    [Table(nameof(HrmsEmployeeAppraisal))]
    public class HrmsEmployeeAppraisal : HrmsEmployeeSubPart
    {
        public string RevisedDesignation { get; set; }
        [Required] public DateTime AppraisalDate { get; set; }
        [Required] public int RevisedCtc { get; set; }
    }

    [Table(nameof(HrmsEmployeeSalary))]
    public class HrmsEmployeeSalary : HrmsEmployeeSubPart
    {
        [Required] public DateTime Date { get; set; }
        [Required] public int WorkingDays { get; set; }
        [Required] public int Leaves { get; set; }
        [Required] public int FixSalary { get; set; }
        [Required] public int Bonus { get; set; }
        public string Description { get; set; }
        [NotMapped] public string ViewDate => Date.ToString("dd-MM-yyyy");
        [NotMapped] public int TotalAmount => FixSalary + Bonus;
    }

    [Table(nameof(HrmsEmployeeDocument))]
    public class HrmsEmployeeDocument : HrmsEmployeeSubPart
    {
        [Required] public string DocumentName { get; set; }
        public string Number { get; set; }
        public string DocumentUrl { get; set; }
    }

    [Table(nameof(HrmsEmployeeBank))]
    public class HrmsEmployeeBank : HrmsBank
    {
        [Required] public int EmployeeId { get; set; }
        [JsonIgnore][ForeignKey(nameof(EmployeeId))] public HrmsEmployee Employee { get; set; }
    }

    [Table(nameof(HrmsEmployeePunching))]
    public class HrmsEmployeePunching : HrmsEmployeeSubPart
    {
        [Required] public DateTime PunchTime { get; set; } = DateTime.Now;
        [Required] public int PunchType { get; set; }
    }
}
