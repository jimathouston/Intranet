using Intranet.Web.Authentication.Contracts;
using Intranet.Web.Authentication.Models;
using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Options;

namespace Intranet.Web.Authentication.Services
{
    public class LdapAuthenticationService : IAuthenticationService
    {
        private const string MemberOfAttribute = "memberOf";
        private const string DisplayNameAttribute = "displayName";
        private const string SAMAccountNameAttribute = "sAMAccountName";
        private const string EMailAttribute = "mail";

        private readonly LdapConfig _config;
        private readonly LdapConnection _connection;

        public LdapAuthenticationService(IOptions<LdapConfig> config)
        {
            _config = config.Value;
            _connection = new LdapConnection
            {
                SecureSocketLayer = false,
            };
        }

        public IUser VerifyUser(string username, string password)
        {
            try
            {
                username = username.ToLower();

                if (username.Contains('@'))
                {
                    username = username.Remove(username.IndexOf('@'));
                }

                _connection.Connect(_config.Url, this.Port);
                _connection.Bind(CN, _config.BindCredentials);

                var searchFilter = string.Format(_config.SearchFilter, username);
                var result = _connection.Search(
                    @base: _config.SearchBase,
                    scope: LdapConnection.SCOPE_SUB,
                    filter: searchFilter,
                    attrs: new[] { SAMAccountNameAttribute, DisplayNameAttribute, MemberOfAttribute, EMailAttribute },
                    typesOnly: false,
                    cons: new LdapSearchConstraints
                    {
                        ReferralFollowing = true,
                    }
                );

                var user = result.hasMore() ? result.next() : null;
                if (user != null)
                {
                    _connection.Bind(user.DN, password);
                    if (_connection.Bound)
                    {
                        return new User
                        {
                            DisplayName = user.getAttribute(DisplayNameAttribute)?.StringValue ?? String.Empty,
                            Username = user.getAttribute(SAMAccountNameAttribute).StringValue,
                            Email = user.getAttribute(EMailAttribute).StringValue,
                            IsAdmin = user.getAttribute(MemberOfAttribute)?.StringValueArray.Contains(_config.AdminCn) == true,
                            IsDeveloper = user.getAttribute(MemberOfAttribute)?.StringValueArray.Contains(_config.DeveloperCn) == true,
                        };
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                _connection.Disconnect();
            }
        }

        private int Port => _connection.SecureSocketLayer ? LdapConnection.DEFAULT_SSL_PORT : LdapConnection.DEFAULT_PORT;
        private string CN => _config.BindDn?.Split(',')?.SingleOrDefault(s => s.StartsWith("CN"))?.Replace("CN=", String.Empty);
    }
}
