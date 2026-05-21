namespace MaBoutique.Session;

public interface IUserSessionTokenStore
{
    string? GetJwtToken();
    DateTime? GetJwtExpiration();
    void SetJwt(string token, DateTime expiration);
    void ClearJwt();
}
