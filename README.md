# Intranet

Certaincy's intranet, built with .Net Core and Angular

## Environment variables

| Key                    | Value                          |
|------------------------|--------------------------------|
| LDAP_ADMIN_CN          |                                |
| LDAP_BIND_CREDENTIALS  |                                |
| LDAP_BIND_DN           |                                |
| LDAP_DEVELOPER_CN      |                                |
| LDAP_SEARCH_BASE       |                                |
| LDAP_SEARCH_FILTER     |                                |
| LDAP_URL               |                                |
| INTRANET_JWT           |                                |
| ASPNETCORE_ENVIRONMENT | Development\|Staging\|Production |

## Development

### Migrations

Go to `src\Intranet.API\Intranet.API.Domain` and run `dotnet ef` with `Intranet.API` as the startup project to generate new migrations:

```
> dotnet ef --startup-project ..\Intranet.API\ migrations add {Name Of Migration}
```

Running the web api automatically applies the migration. To apply it manually run the following:

```
> dotnet ef --startup-project ..\Intranet.API\ database update
```

### Client App

_Note: You must be in `src/Intranet.Web/Intranet.Web` to use the npm scripts._

#### Lint

The client app can be linted by running:

```
> npm run lint
```

Some linting issues can be fixed automatically by `tslint`:

```
> npm run lint:fix
```

#### Unit Testing

Karma is used for unit testing.

Run the unit tests with:

```
> npm test
```

#### First time setup

Before the Angular app runs the first time the bundles must be built with Webpack. After installing all dependencies the app will be built automatically:

```
> npm install
```
