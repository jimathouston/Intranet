namespace Intranet.Web.Contracts
{
  public interface IUser
  {
    string AuthenticationType { get; }
    string FirstName { get; set; }
    bool IsVerified { get; }
    string Sid { get; }
  }
}