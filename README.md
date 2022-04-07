# StarterPack .NetCore

## A base web application pack for the development of web applications, implemented common flows, controllers, services and components necessary for most web applications. It uses active record standard with Entity Framework for data access and persistence.

# Mains reources
- Custom CLI extension
- Migrations and seed
- Auto generation of migration based on models
- Tests
- Automated deployment
- Authentication/login
- Roles and permissions
- Base controllers
- Treated/custom exceptions
- Auto validation for events that use the base controller

## Commands

### start the server
- dotnet watch run

### Load the added libraries
- dotnet restore

### Create a migration to reflect the state of the models
- dotnet ef migrations add NameOfMigration

### Delete the last generated migration
- dotnet ef migrations remove

### Generates a .sql file referring to the content of migrations
- dotnet ef migrations script

### Run migrations
- dotnet ef database update
- dotnet ef database update -e Local

### Run tests
- dotnet test ou dotnet xunit

### List tests
- dotnet test -t

### To check StarterPack CLI options
- dotnet run sp -h

### To see all StarterPack CLI seed options
- dotnet run sp seed -h

### To run the StarterPack seed
- dotnet run sp seed

### To run StarterPack CLI seed resetting data
- dotnet run sp seed --reset

### To run the StarterPack deployment
- dotnet run sp deploy
