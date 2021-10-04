using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Api.Services
{
    public class UserService : IUserService, IService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid AdminId => new Guid("24e2a132-c2e1-4a11-97a1-4dfb2fc524a0");
        public Guid? UserId
        {
            get
            {
                var id = ClaimUserlogged(Claims.Subject);
                return id == null ? (Guid?) null : Guid.Parse(id);
            }
        }
        public string ClaimUserlogged(string claimName)
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(claimName);
        }
    }
}