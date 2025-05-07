# Internet Store ASP.NET Project based on Dropshipping Model
Work in progress.

## Milestones
All expected features has been implemented, current work revolves around fixing bugs and upgrades. \
✅ Searchbar \
✅ Filters \
✅ Categories \
✅ User registration and authorization \
✅ Page size and page select \
✅ Product ordering and basket \
✅ Order summarization \
✅ Order review \
✅ Fluent validation \
✅ Product view page \
⏳ UI changes \
⏳ Proper JWT handling \
⏳ Server-side tests \
⏳ Hotfixes

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
- Fluent Validation
  - FluentValidation
  - FluentValidation.AspNetCore
- JSON Web Token
  - Microsoft.AspNetCore.Authentication.JwtBearer
- React Router
- Vite

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
