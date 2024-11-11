using Microsoft.AspNetCore.Identity;

namespace PepeWeb.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string Nickname
        {
            get
            {
                // Get the part before the '@' symbol in the email
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
