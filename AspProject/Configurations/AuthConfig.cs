namespace AspProject.Configurations;

public class AuthConfig
{
    public int ExpiredAtMinutes { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string IssuerSignKey { get; set; }
}
