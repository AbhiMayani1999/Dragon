using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dragon.Model.Configs
{
    [Table($"Platform{nameof(KeyGroup)}")]
    public class KeyGroup
    {
        [Key] public int Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public bool IsGeneric { get; set; } = false; // If true then can not be deleted

        public List<KeyStore> KeyStore { get; set; }
    }

    [Table($"Platform{nameof(KeyStore)}")]
    public class KeyStore
    {
        [Key] public int Id { get; set; }
        [Required] public string Key { get; set; }
        [Required] public string Value { get; set; }
        public string SubValue { get; set; }

        [Required] public int KeyGroupId { get; set; }
        [ForeignKey(nameof(KeyGroupId))] public KeyGroup KeyGroup { get; set; }
    }
}
