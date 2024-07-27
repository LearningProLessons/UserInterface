namespace UserInterface;
public class AuthenticationSettings
{
    public string Authority { get; set; }
    public string SignOutCallbackPath { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string ResponseType { get; set; }
    public List<string> Scopes { get; set; }
    public JwtBearerSettings JwtBearer { get; set; }
}

public class JwtBearerSettings
{
    public string Audience { get; set; }
    public bool RequireHttpsMetadata { get; set; }
    public string IssuerSigningKey { get; set; }
}
