using ProjectTaskManagement.Domain.Common;
using ProjectTaskManagement.Domain.Entities.Identity;

namespace ProjectTaskManagement.Domain.Entities.Auth;

public class RefreshToken : BaseEntity
{
    public required string Token { get; set; }
    public DateTime Expires_On { get; set; }
    public bool Is_Expired => DateTime.UtcNow >= Expires_On;
    public string User_Id { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;
}
