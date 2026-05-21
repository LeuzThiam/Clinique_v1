namespace MaBoutique.Auth.Infrastructure.Options;

public class UsersClientOptions
{
    public const string SectionName = "UserService";
    public string BaseUrl { get; set; } = "http://localhost:5002";
}
