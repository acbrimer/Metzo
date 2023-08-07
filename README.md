# Metzo

A lightweight, unobtrusive layer between Entity Framework and a simple, well-defined REST Api

## About
This project is still, as you may notice, very much still in development. I have been working on this concept for several years and finally figured out a good solution for making it work using C# + Entity Framework. I still have several weekends worth of coding before this is ready to use, but feel free to check it out and stay tuned for more!

## Project Overview

\
├── Metzo.sln
├── Core/
│ ├── Core.csproj
│ └── // Core project files
├── SimpleREST/
│ ├── SimpleREST.csproj
│ └── // SimpleREST project files
├── Console/
│ ├── console.csproj
│ └── // Console application files
├── Test/
│ ├── Core.Tests/
│ │ ├── Core.Tests.csproj
│ │ └── // Core unit tests files
│ └── SimpleREST.Tests/
│ ├── SimpleREST.Tests.csproj
│ └── // SimpleREST unit tests files
└── Demo/
├── DbContexts/
│ └── DemoDbContext/
│ ├── DemoDbContext.csproj
│ └── // Demo DbContext project files
└── Apps/
└── DemoApp/
├── DemoApp.csproj
├── // Demo Web API project files
└── Client/
├── package.json
└── // Client SPA files
