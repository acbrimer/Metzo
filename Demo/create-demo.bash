#!/bin/bash

demoName=$1
demoDbContextNamespace="Metzo.Demo.DbContexts.${demoName}DbContext"
demoAppNamespace="Metzo.Demo.Apps.${demoName}App"

# Part 1: Create a dotnet class library and add necessary packages
mkdir "DbContexts/${demoName}DbContext"
dotnet new classlib -n $demoDbContextNamespace -o "DbContexts/${demoName}DbContext"
cd "DbContexts/${demoName}DbContext"
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package MySql.Data.EntityFramework
cd ../../..

# Part 2: Add new classlib to unit test projects and console
dotnet sln "Metzo.sln" add "Demo/DbContexts/${demoName}DbContext/${demoDbContextNamespace}.csproj"
dotnet add "Tests/Core.Tests/Metzo.Core.Tests.csproj" reference "Demo/DbContexts/${demoName}DbContext/${demoDbContextNamespace}.csproj"
dotnet add "Tests/SimpleREST.Tests/Metzo.SimpleREST.Tests.csproj" reference "Demo/DbContexts/${demoName}DbContext/${demoDbContextNamespace}.csproj"
dotnet add "Console/Metzo.Console.csproj" reference "Demo/DbContexts/${demoName}DbContext/${demoDbContextNamespace}.csproj"


cd Demo
dotnet new webapi -n $demoAppNamespace -o "Apps/${demoName}App"
cd "Apps/${demoName}App"
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package MySql.Data.EntityFramework
dotnet add reference ../../../Core/Metzo.Core.csproj
dotnet add reference ../../../SimpleREST/Metzo.SimpleREST.csproj
dotnet add reference ../../DbContexts/${demoName}DbContext/Metzo.Demo.DbContexts.${demoName}DbContext.csproj


npm init vite@latest client -- --template react-ts -y -f
cd client
demoNameLower=$(tr '[:upper:]' '[:lower:]' <<< "$string")
demoClientName="@metzo/demo/${demoNameLower}-app"
node -e "
const fs = require('fs');
let rawdata = fs.readFileSync('package.json');
let package = JSON.parse(rawdata);
package['name'] = \"${demoClientName}\";
fs.writeFileSync('package.json', JSON.stringify(package, null, 2));
"
npm install -y -f
npm install react-admin ra-data-simple-rest
