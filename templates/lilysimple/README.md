# LilySimple

## Features

- Soft Deletion in entity model base
- Database schema auto init at first time run
- JwtBearer Authentication
- Swagger Doc
- Hangfire for background jobs
- Auto registration for all business services
- Global exception handler
- Request logs

## DesignTime EFCore Migration

```powershell
cd .\src\LilySimple.Infra
# Select `LilySimple.Infra` as the target project.
dotnet ef migrations add $MIGRATION_NAME -o .\Migrations 
dotnet ef database update
```