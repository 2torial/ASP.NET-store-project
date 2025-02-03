# Internet Store ASP.NET Project for College Classes
Work in progress. \
\
Stage of the project changed from *setting up* to *adding features*. \
Next phase is going to focus on UI overhaul and implementing tests.

## Milestones
:white_check_mark: Working searchbar \
:white_check_mark: User registration and authorization \
:white_check_mark: Item ordering and basket functionalities \
:white_check_mark: Expanded database \
:white_check_mark: Order summarization \
:hourglass: Adding/editing/removing items (admin panel) \
:white_check_mark: User list (admin panel) \
:white_check_mark: Order review (admin panel) \
\
Some time is going to be devoted to improving already implemented features.

## Technologies Used
- ASP.NET Core 8
- PostgreSQL Database (local, set up through EF migration)
- React, JSX
- SASS*, TypeScript \
\
*Yet to be implemented

### Other Tools Incorporated (with required NuGet packages)
- Entity Framework
  - Microsoft.EntityFrameworkCore
  - Microsoft.EntityFrameworkCore.Tools
  - Npgsql.EntityFrameworkCore.PostgreSQL
- JSON Web Token
  - Microsoft.AspNetCore.Authentication.JwtBearer
- React Router
- Vite \
\
*Yet to be implemented

### Testing and CI/CD
Carried out using NUnit* and GitHub Actions*. \
\
*Yet to be implemented

### Set Up
Project requires to Add-Migration, Update-Database through Entity Framework to work if it's run for the first time (database is going to be named *Store*).
Program requires the user to input correct database login and password in the appsettings.config file to run. 
There are two preset site accounts: user (password: user) and root (password: root), along other mockup data included.

## Contributors
Daniel Boguszewski (315942)
