# lily

Some frameworks for rapid development.

- [lilynormal](templates/lilynormal/README.md)
- [lilysimple](templates/lilysimple/README.md)

## 1. Install template

Change directory to the root of template

```powershell
# . indicates the root
dotnet new -i .
```

To see if installed successfully

```powershell
dotnet new -l
```

## 2. Usage

At somewhere else

```powershell
dotnet new lily{templateName} -n $YOUR_PROJECT_NAME
```

## 3. Remove template

To see the remove command

```powershell
dotnet new -u
```

Find and execute it