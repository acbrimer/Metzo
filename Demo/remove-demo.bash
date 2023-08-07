#!/bin/bash

demoName=$1
demoDbContextNamespace="Metzo.Demo.DbContexts.${demoName}DbContext"

cd ..
dotnet sln "Metzo.sln" remove "Demo/DbContexts/${demoName}DbContext/${demoDbContextNamespace}.csproj"
dotnet remove "Tests/Core.Tests/Metzo.Core.Tests.csproj" reference "Demo/DbContexts/${demoName}DbContext/${demoDbContextNamespace}.csproj"
dotnet remove "Tests/SimpleREST.Tests/Metzo.SimpleREST.Tests.csproj" reference "Demo/DbContexts/${demoName}DbContext/${demoDbContextNamespace}.csproj"
dotnet remove "Console/Metzo.Console.csproj" reference "Demo/DbContexts/${demoName}DbContext/${demoDbContextNamespace}.csproj"
rm -dR "Demo/Apps/${demoName}App"
rm -dR "Demo/DbContexts/${demoName}DbContext"