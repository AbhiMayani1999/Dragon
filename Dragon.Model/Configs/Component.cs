using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dragon.Model.Configs
{
    [Table($"Platform{nameof(ComponentProperty)}")]
    public class ComponentProperty
    {
        [Key] public int Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public bool IsGeneric { get; set; } = false;
    }

    [Table($"Platform{nameof(ComponentStructure)}")]
    public class ComponentStructure
    {
        [Key] public int Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public bool IsGeneric { get; set; } = false;
        [Required] public bool CanBeParent { get; set; } = false;

        public List<ComponentStructureProperty> Properties { get; set; }
        [NotMapped] public List<OptionGType<int, bool>> PropertyList { get; set; }
        [NotMapped] public List<int?> ParentMapping { get; set; }
        [NotMapped] public List<int?> ChildMapping { get; set; }
    }

    [Table($"Platform{nameof(ComponentStructureSubComponent)}")]
    public class ComponentStructureSubComponent
    {
        [Key] public int Id { get; set; }
        public int? ChildComponentId { get; set; }
        public int? ParentComponentId { get; set; }
    }

    [Table($"Platform{nameof(ComponentStructureProperty)}")]
    public class ComponentStructureProperty
    {
        [Key] public int Id { get; set; }
        [Required] public bool IsMultiple { get; set; } = false;

        public int? ComponentStructureId { get; set; }
        [ForeignKey(nameof(ComponentStructureId))] public ComponentStructure ComponentStructure { get; set; }

        public int? ComponentPropertyId { get; set; }
        [ForeignKey(nameof(ComponentPropertyId))] public ComponentProperty ComponentProperty { get; set; }
    }


    [Table($"Platform{nameof(Structure)}")]
    public class Structure
    {
        [Key] public int Id { get; set; }

        [Required] public string Identifier { get; set; }
        public int? ComponentStructureId { get; set; }
        [ForeignKey(nameof(ComponentStructureId))] public ComponentStructure ComponentStructure { get; set; }

        public List<StructureProperty> Properties { get; set; }

        public int? ShortIndex { get; set; }

        [NotMapped] public List<int?> ParentMapping { get; set; }
        [NotMapped] public List<int?> ChildMapping { get; set; }
        [NotMapped] public int ParentStructureId { get; set; }
    }

    [Table($"Platform{nameof(StructureSubStructure)}")]
    public class StructureSubStructure
    {
        [Key] public int Id { get; set; }
        public int? ChildStructureId { get; set; }
        public int? ParentStructureId { get; set; }
    }

    [Table($"Platform{nameof(StructureProperty)}")]
    public class StructureProperty
    {
        [Key] public int Id { get; set; }
        public int? ComponentStructurePropertyId { get; set; }
        [ForeignKey(nameof(ComponentStructurePropertyId))] public ComponentStructureProperty ComponentStructureProperty { get; set; }
        [Required] public string Type { get; set; }
        public string Value { get; set; }

        public int? StructureId { get; set; }
        [NotMapped] public List<int?> ChildMapping { get; set; }
    }

    [Table($"Platform{nameof(StructurePropertyToStructure)}")]
    public class StructurePropertyToStructure
    {
        [Key] public int Id { get; set; }
        public int? ChildStructureId { get; set; }
        public int? StructurePropertyId { get; set; }
    }
}