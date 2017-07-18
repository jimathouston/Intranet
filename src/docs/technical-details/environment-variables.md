# Environment variables

| Key                    | Value                            |
|------------------------|----------------------------------|
| AWS_ACCESS_KEY_ID      |                                  |
| AWS_BUCKET_NAME        |                                  |
| AWS_SECRET_ACCESS_KEY  |                                  |
| AWS_REGION             |                                  |
| LDAP_ADMIN_CN          |                                  |
| LDAP_BIND_CREDENTIALS  |                                  |
| LDAP_BIND_DN           |                                  |
| LDAP_DEVELOPER_CN      |                                  |
| LDAP_SEARCH_BASE       |                                  |
| LDAP_SEARCH_FILTER     |                                  |
| LDAP_URL               |                                  |
| CONNECTION_STRING      | Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True; |
| INTRANET_JWT           |                                  |
| ASPNETCORE_ENVIRONMENT | Development\|Staging\|Production |

## dotnet user-secrets

It's possible to use `dotnet user-secrets` to add enviroment variables to single projects (instead of having them global on the machine). For example:

```
> cd .\src\Intranet.API\Intranet.API\
> dotnet user-secrets set CONNECTION_STRING "Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;"
```

Quotation marks is not necessary for "simple" strings:

```
> dotnet user-secrets set AWS_REGION eu-central-1
```