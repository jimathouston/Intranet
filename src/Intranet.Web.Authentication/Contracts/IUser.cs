namespace Intranet.Web.Authentication.Contracts
{
    public interface IUser
    {
        string DisplayName { get; set; }
        bool IsAdmin { get; set; }
        bool IsDeveloper { get; set; }
        string Username { get; set; }
        string Email { get; set; }
    }
}