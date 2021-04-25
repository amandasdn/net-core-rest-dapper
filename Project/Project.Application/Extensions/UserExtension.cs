using Microsoft.AspNetCore.Http;
using Project.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Project.Application.Extensions
{
    /// <summary>
    /// User Extension.
    /// </summary>
    public class UserExtension : IUser
    {
        private readonly IHttpContextAccessor _accessor;

        /// <summary>
        /// User Extension.
        /// </summary>
        public UserExtension(IHttpContextAccessor accessor) => _accessor = accessor;

        /// <summary>
        /// Get the logged user's name.
        /// </summary>
        // public string Name => _accessor.HttpContext.User.Identity.Name;

        /// <summary>
        /// Get the logged user's ID (Guid).
        /// </summary>
        public Guid GetUserId() => IsAuthenticated() ? Guid.Parse(_accessor.HttpContext.User.GetUserId()) : Guid.Empty;

        /// <summary>
        /// Get the logged user's email.
        /// </summary>
        public string GetUserEmail() => IsAuthenticated() ? _accessor.HttpContext.User.GetUserEmail() : string.Empty;

        /// <summary>
        /// Check if user is authenticated.
        /// </summary>
        public bool IsAuthenticated() => _accessor.HttpContext.User.Identity.IsAuthenticated;
    }

    /// <summary>
    /// Claim Principal Extensions.
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Get the logged user's ID (Guid).
        /// </summary>
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value;
        }

        /// <summary>
        /// Get the logged user's email.
        /// </summary>
        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst(ClaimTypes.Email);
            return claim?.Value;
        }
    }
}
