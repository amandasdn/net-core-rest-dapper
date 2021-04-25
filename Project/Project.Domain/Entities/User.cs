using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Domain.Entities
{
    public class User
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }

    public class UserResponse
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }
    }

    public class UserTokenResponse
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<ClaimResponse> Claims { get; set; }
    }

    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public double ExpiresIn { get; set; }
        public UserTokenResponse UserToken { get; set; }
    }

    public class ClaimResponse
    {
        public string Value { get; set; }
        public string Type { get; set; }
    }
}
