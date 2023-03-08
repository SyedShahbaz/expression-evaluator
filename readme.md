
# UL Solutions Expression Evaluator

This project is a mathematical expression evaluator interface that evaluates a string expression consisting of non-negative integers and the + - / * operators only.  

## Technologies

- .NET 6
- Web API
- Docker

## Design Practices
- Clean Architecture
- Decorator Pattern 
- SOLID principles
- MOQ Unit Tests
- Infrastructure Testing

## Features 
- WatchDog 
- SwaggerGen
- Health Check 
- Rate Limit
- Request Caching
- Security (password protection via environment variables)
- API Rate Limit
- On demand request caching (enable/disable request caching via configuration)
- Logging 
- Exception Handling
- AutoMapper

## Pre-requisites

Installation of .NET 6, Docker and any C# editor (preferably VSCode, Rider or Visual Studio)

## How to Run Locally
#### Via Editor
 Build solution and run, navigate to following URLs to see the API in Swagger and view the logs in WatchDog. 

- If you are using Visaul Studio it will navigate to the SwaggerUI automatically. For VS for Mac it automatically redirects to the following URL: https://localhost:7129/swagger/index.html

- If you are using rider you need to manually navigate to the below URLS
- https://localhost:5001/swagger/index.html
- https://localhost:5001/watchdog
	- Username: admin, Password: admin. 
	- Also to be given in Program.cs while registeration via environment variables. I have commented out that code and hardcoded the credentials for your ease as otherwise you will need to set environment variables WATCHDOG_USERNAME and WATCHDOG_PASSWORD with the above values. 

#### Via Docker
navigate to root directory where Dockerfile is located and run the following commands. 

- docker build -t my-app .
- docker run -e WATCHDOG_USERNAME=admin -e WATCHDOG_PASSWORD=admin -p 8080:80 my-app
	- This passes in the environment variables too
	
Then Navigate to these URLs. 

- http://localhost:8080/swagger/index.html
- http://localhost:8080/watchdog

## Architecture Diagram
Clean architecture is followed for this project. A high level architecture diagram for the solution presented is given below.  
![Alt text](ULSolutions.png?raw=true "Architecture Diagram")

## How I Approached the Problem 

This solution is made by using clean architecture as clean architecture emphasizes separation of concerns, with a focus on making the architecture of the system clear, modular, and easy to understand. The goal of clean architecture is to create software systems that are flexible, maintainable, and testable.

Apart from that, I have followed SOLID principles, especially Dependency Injection. Which makes testing very easy, makes code reusable by separating the dependencies between the components, promotes loose coupling and extensibility. 

I have also added Unit Testing to check the desired behavior of the code and eliminate any errors that might occur.

I have also added Infrastructure testing. That is to check that code doesn't break the infrastructure rules. i.e. Core Project should not be dependent on Web Project. We can add more test cases to make sure that code follows the agreed upon guidelines. e.g. every service in the core project should have a name ending with 'Service' etc.

If this code is deployed to a public cloud with public access we also want to make sure that nobody misuses our resources. For that I have also added the Web Request Rate
Limiter as well. What it does is that it will not let anyone make more than 200 requests in a minute. This can be altered and enhanced according to the needs.

I have also thought that ExpressionEvaluatorService is resource expensive and sometime later, we might decide to replace it with azure functions, with which every request has a cost tag. So, to minimize that cost I have also added request caching. Let's say we have the same expression coming from the request in an interval, instead of calling the resource expensive computation again and again, we can make use of caching and save the cost. To achieve this dynamically, I have added a Decorator around the ExpressionEvaluatorService which can be enabled or disabled on demand from the configuration file (appsettings.json).

I have also added Docker support in the solution, with which we can deploy the application via docker image as well and run it locally via docker too. 

As logging is always critical in any application, I have added WatchDog to monitor logs of the application. It provides a UI for that and requires a password and username which I have saved in the Environment Variables. We can easily set the environment variables in the deployment environment. Let it be Azure app service, AWS or some VM.

Monitoring the health of the application and services used by the application is very important in any large scale application. For that I have added a Health Check feature which can be extended to monitor infrastructureand services health along with the API itself. 

Overall I have tried to write clean code, small testable classes and functions.

