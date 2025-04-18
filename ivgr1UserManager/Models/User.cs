namespace ivgr1UserManager.Models;

public class User
{
    public int id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    public string NotificationPreference { get; set; }
    public string AccountType { get; set; }
    public bool IsTermsAccepted { get; set; }
}