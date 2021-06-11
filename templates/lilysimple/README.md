# LilySimple

## DesignTime EFCore Migration 

```powershell
cd .\src\LilySimple.Infra
# Select `LilySimple.Infra` as the target project.
dotnet ef migrations add $MIGRATION_NAME -o .\Migrations 
dotnet ef database update
```