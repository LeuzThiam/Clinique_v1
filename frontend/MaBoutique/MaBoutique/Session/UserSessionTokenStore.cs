using Microsoft.AspNetCore.Http;

namespace MaBoutique.Session;

public class UserSessionTokenStore : IUserSessionTokenStore
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserSessionTokenStore(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetJwtToken() => _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");

    public DateTime? GetJwtExpiration()
    {
        var raw = _httpContextAccessor.HttpContext?.Session.GetString("JwtTokenExpiration");
        return DateTime.TryParse(raw, out var value) ? value : null;
    }

    public void SetJwt(string token, DateTime expiration)
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session == null) return;
        session.SetString("JwtToken", token);
        session.SetString("JwtTokenExpiration", expiration.ToString("O"));
    }

    public void ClearJwt()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        session?.Remove("JwtToken");
        session?.Remove("JwtTokenExpiration");
    }
}
