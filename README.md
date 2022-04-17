# Getting Started

## Reference Documentation

Requirements: 

 - [ASP.NET Core Runtime 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

Optional:
- Visual Studio 2022
 
## Full API
To run the application go to path **CTeleport-Exercise/Api/src/CTeleport.Exercise.Api** and type the commands:

    dotnet build

    dotnet run

#### Endpoints
To call the endpoints, the queries **origin** and **destiny** are mandatory and it is case insensitive:
**GET**: https://localhost:5001/api/airports/distance?Origin=gru&Destiny=bsb
or
**GET**: http://localhost:5000/api/airports/distance?Origin=gru&Destiny=bsb

You can see the swagger project in:
http://localhost:5000/swagger


## Minimal API
### kestrel
To run the minimal api go to path **CTeleport-Exercise/CTeleport.Exercise.MinimalApi/CTeleport.Exercise.MinimalApi** and time the commands:

    dotnet build

    dotnet run

The endpoints to test are:
**GET**: https://localhost:7038/airports/distance?origin=gru&destiny=cwb
or
**GET**: http://localhost:5230/airports/distance?origin=gru&destiny=cwb


### docker
To run the minimal api with docker, go to path **CTeleport-Exercise/CTeleport.Exercise.MinimalApi** and execute the commands:

    docker build -t cteleportminimalapi -f Dockerfile .
    docker run -p 5000:80 cteleportminimalapi

You can call the api with the endpoint:
**GET**: http://localhost:5000/airports/distance?origin=gru&destiny=cwb

It is also possible to execute the API in Development Environment with VisualStudio. Just select "Docker" to execute the project and it will open the swagger window with the api address. 