namespace UserInterface;
public class AuthenticationSettings
{
    public string Authority { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string ResponseType { get; set; }
    public List<string> Scopes { get; set; }
}
