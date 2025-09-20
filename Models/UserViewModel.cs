namespace GocPho.Models;

public class UserViewModel
{
    public string Id { get; set; } = "";
    public string UserName { get; set; } = "";
    public string Email { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
    public bool EmailConfirmed { get; set; }
    public List<string> Roles { get; set; } = new();
}
