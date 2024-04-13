using _535_Assignment.Models;
using _535_Assignment.Service;
using Microsoft.AspNetCore.Identity;

namespace _535_Assignment.Repository
{
    public enum Roles
    {
        Admin,
        User
    }

    public class AuthRepository
    {
        private readonly ContextShopping _context;
        private readonly SanitiserService _sanitiserService;
        public AuthRepository(ContextShopping context, SanitiserService sanitiserService)
        {
            _context = context;
            _sanitiserService = sanitiserService;   
        }

        /// <summary>
        /// Sanitises user details before verifying their encryption hash to match.
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        public AppUser Authenticate(LoginDTO credentials)
        {
            var sanitisedUsername = _sanitiserService.Sanitiser.Sanitize(credentials.user);
            var sanitisedPassword = _sanitiserService.Sanitiser.Sanitize(credentials.password);

            var userDetails = GetUserByUsername(sanitisedUsername);

            if (userDetails == null)
            {
                return null;
            }

            if (BCrypt.Net.BCrypt.EnhancedVerify(sanitisedPassword, userDetails.Password))
            {
                return userDetails;
            }
            return null;

        }

        /// <summary>
        /// Finds a user in the database by username.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private AppUser GetUserByUsername(string userName)
        {
            var user = _context.AppUsers.Where(c => c.UserName.Equals(userName)).FirstOrDefault();

            return user;
        }


    }
}
