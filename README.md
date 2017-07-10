# Intranet

Certaincy's intranet, built with .Net Core and Angular

## Environment variables

| Key                    | Value                            |
|------------------------|----------------------------------|
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


### AWS RDS
Create Snapshot (Example: Of Production) 
```
> aws rds create-db-snapshot --db-instance-identifier <dbidentifiername> --db-snapshot-identifier <snapshotidentifier> 
```

Delete Snapshot (Example: Of Production) 
```
> aws rds delete-db-snapshot --db-snapshot-identifier <name> 
```

Restore Instance from snapshot(Create a new instance from snapshot, Example: Staging Database)  
```
> aws rds restore-db-instance-from-db-snapshot --db-instance-identifier <mynewdbinstance> --db-snapshot-identifier <mydbsnapshot> 
```

Delete Instance (Example: Staging) without: final Snapshot 
```
> aws rds delete-db-instance --db-instance-identifier <mydbinstance> --skip-final-snapshot 
```

Delete Instance with final Snapshot 
```
> aws rds delete-db-instance --db-instance-identifier <mydbinstance> --final-snapshot-identifier <myfinaldbsnapshot> 
``` 

### DB for developement
Note: A local DB must already exist. 
From SQL Server Managment:
Right-Click the DB you want to copy.  
Select Tasks. 
Click Export Data.  
Follow Wizard to copy data from cloud sever to local.  

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
