using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dragon.Model.Configs
{
    [Table($"Config{nameof(DomainConnect)}")]
    public class DomainConnect
    {
        [Key] public int Id { get; set; }
        [Required] public string Name { get; set; }
        public string TenantCode { get; set; }
    }

    [Table($"Config{nameof(DomainSetting)}")]
    public class DomainSetting
    {
        [Key] public int Id { get; set; }
        [Required] public string BrandName { get; set; }
        public string LogoUrl { get; set; }
        public string Address { get; set; }
        public string Tagline { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        [Required] public bool IsActive { get; set; } = true;

        [Required] public int DomainConnectId { get; set; }
        [ForeignKey(nameof(DomainConnectId))] public DomainConnect DomainConnect { get; set; }
    }
}