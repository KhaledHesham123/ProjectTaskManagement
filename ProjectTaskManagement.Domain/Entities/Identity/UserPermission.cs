namespace ProjectTaskManagement.Domain.Entities.Identity;

public class UserPermission
{
    public Guid Id { get; set; }
    public string User_Id { get; set; } = string.Empty;
    public string Permission { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;
}
