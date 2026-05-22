namespace ProjectTaskManagement.Domain.Entities.Identity;

public class RolePermission
{
    public Guid Id { get; set; }
    public string Role_Id { get; set; } = string.Empty;
    public string Permission { get; set; } = string.Empty;

    public ApplicationRole Role { get; set; } = null!;
}
