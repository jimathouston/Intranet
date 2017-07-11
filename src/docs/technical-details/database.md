# Database

## Migrations

Go to `src\Intranet.API\Intranet.API.Domain` and run `dotnet ef` with `Intranet.API` as the startup project to generate new migrations:

```
> dotnet ef --startup-project ..\Intranet.API\ migrations add <NameOfMigration>
```

Running the web api automatically applies the migration. To apply it manually run the following:

```
> dotnet ef --startup-project ..\Intranet.API\ database update
```


## AWS RDS
Create Snapshot (Example: Of Production) 
```
> aws rds create-db-snapshot --db-instance-identifier <mydbinstance> --db-snapshot-identifier <mydbsnapshot> 
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

## DB for developement
_Note: A local DB must already exist._

From SQL Server Managment:

* Right-Click the DB you want to copy
* Select Tasks
* Click Export Data
* Follow Wizard to copy data from cloud sever to local