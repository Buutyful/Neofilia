using Microsoft.AspNetCore.Identity;
using Neofilia.Domain;

namespace Neofilia.Server.Data;


public class ApplicationUser : IdentityUser
{
    public ICollection<Local> ManagedLocals { get; private set; } = [];
}
