using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dragon.Model.Configs
{
    public abstract class NavigationBase
    {
        [Key] public int Id { get; set; }
        public string State { get; set; }
        [Required] public string Name { get; set; }
        [Required] public int SortIndex { get; set; }
        [Required] public string Code { get; set; }
        public string Icon { get; set; }
        [Required] public bool IsActive { get; set; } = true;

        [NotMapped] public bool Disabled { get; set; }
        [NotMapped] public bool IsCreate { get; set; } = true;
        [NotMapped] public bool IsEdit { get; set; } = true;
        [NotMapped] public bool IsView { get; set; } = true;
        [NotMapped] public bool IsDelete { get; set; } = true;
    }

    [Table($"Platform{nameof(Navigation)}")]
    public class Navigation : NavigationBase
    {
        public List<Navigation> ChildItems { get; set; }

        public int? ParentNavigationId { get; set; }
        [ForeignKey(nameof(ParentNavigationId))] public Navigation ParentNavigation { get; set; }
    }

    [Table($"Platform{nameof(UserNavigation)}")]
    public class UserNavigation
    {
        [Key] public int Id { get; set; }
        [Required] public string Code { get; set; }
        [Required] public bool IsCreate { get; set; } = true;
        [Required] public bool IsEdit { get; set; } = true;
        [Required] public bool IsView { get; set; } = true;
        [Required] public bool IsDelete { get; set; } = true;

        [Required] public int UserId { get; set; }
        [ForeignKey(nameof(UserId))] public User Users { get; set; }
    }

    [Table($"Platform{nameof(UserTypeNavigation)}")]
    public class UserTypeNavigation
    {
        [Key] public int Id { get; set; }
        [Required] public string Code { get; set; }

        [Required] public bool IsCreate { get; set; } = true;
        [Required] public bool IsEdit { get; set; } = true;
        [Required] public bool IsView { get; set; } = true;
        [Required] public bool IsDelete { get; set; } = true;

        [Required] public int UserTypeId { get; set; }
        [ForeignKey(nameof(UserTypeId))] public UserType UserTypes { get; set; }
    }
}
