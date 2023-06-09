using Microsoft.AspNetCore.Identity;

namespace Stocks.Core.Identity
{
    /* The class "ApplicationRole" inherits from the "IdentityRole" class and specifies a generic type
    of "Guid". */
    public class ApplicationRole : IdentityRole<Guid>
    {
    }
}

