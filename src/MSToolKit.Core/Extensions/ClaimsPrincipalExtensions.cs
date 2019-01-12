using MSToolKit.Core.Authentication;
using System.Linq;
using System.Security.Claims;

namespace MSToolKit.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for using over System.Security.Claims.ClaimsPrincipal class.
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Returns the string representation of the unique identifier for the current ClaimsPrincipal.
        /// </summary>
        /// <param name="claimsPrincipal">Current System.Security.Claims.ClaimsPrincipal instance</param>
        /// <returns>
        /// The string representation of the unique identifier for the current ClaimsPrincipal.
        /// </returns>
        public static string GetIdentifier(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal
                .Claims
                .FirstOrDefault(cl => cl.Type == AuthenticationConstants.ClaimTypeIdentifier)
                ?.Value;
        }

        /// <summary>
        /// Returns the email for the current ClaimsPrincipal.
        /// </summary>
        /// <param name="claimsPrincipal">Current System.Security.Claims.ClaimsPrincipal instance</param>
        /// <returns>
        /// The email for the current ClaimsPrincipal.
        /// </returns>
        public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal
                .Claims
                .FirstOrDefault(cl => cl.Type == ClaimTypes.Email)
                ?.Value;
        }
    }
}
