using Microsoft.AspNet.Identity.EntityFramework;

namespace AppraisalSln.Models
{
    public class AuthContext : IdentityDbContext<IdentityUser>
    {
        public AuthContext()
            : base("AuthContext")
        {

        }

        public static AuthContext Create()
        {
            return new AuthContext();
        }
    }
}