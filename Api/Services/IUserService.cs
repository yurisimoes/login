using System;

namespace Api.Services
{
    public interface IUserService
    { 
        Guid AdminId { get; }
        Guid? UserId { get; }
        string ClaimUserlogged(string claimName);
    }
}