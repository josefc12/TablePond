using Microsoft.AspNetCore.Identity;

namespace TablePond.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        // Make a nickname from user email.
        public string Nickname
        {
            get
            {
                if (UserName != null)
                {
                    var atIndex = UserName.IndexOf('@');
                    return atIndex > 0 ? UserName.Substring(0, atIndex) : UserName;
                }
                return string.Empty;
            }
        }
    }

}
