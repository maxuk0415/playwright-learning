namespace AutomationLearning.Core.Config;

public class TestSettings
{
    public string BaseUrl { get; set; } = "https://www.saucedemo.com";
    public string ApiBaseUrl { get; set; } = "https://reqres.in";
    public string ApiKey { get; set; } = "";
    public string ValidUsername { get; set; } = "standard_user";
    public string ValidPassword { get; set; } = "secret_sauce";
    public string LockedUsername { get; set; } = "locked_out_user";
    public bool Headless { get; set; } = false;
    public float SlowMo { get; set; } = 0;
}
