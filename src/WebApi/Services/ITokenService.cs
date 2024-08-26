using Dot.Net.WebApi.Domain;

namespace Dot.Net.WebApi.Services
{
    public interface ITokenService
    {
        string CreateToken(User user);

# nullable enable
        int? ValidateToken(string token);
    }
}
